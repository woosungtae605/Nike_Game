using System;
using System.Collections.Generic;
using Agents.CombatSystem;
using Agents.Enemies;
using Agents.Module;
using CoreSystem.BusSystem;
using GameEvents.Camera;
using GameEvents.UI;
using Gamelib.ObjectPool.Runtime;
using Systems;
using UnityEngine;

namespace Agents.Players.Gun.GunData
{
    [Serializable]
    public class RocketLauncherData : GunData
    {
        [Header("Rocket Charge")]
        [SerializeField] private float chargeTime = 1.2f;
        [SerializeField] private float minDamageMultiplier = 1f;
        [SerializeField] private float maxDamageMultiplier = 3f;

        [Header("Explosion")]
        [SerializeField] private float explosionRadius = 3f;
        [SerializeField] private PoolManagerSo poolManagerSo;
        [SerializeField] private PoolItemSo explosionEffect;

        private readonly HashSet<IDamageable> _damagedTargets = new();
        private float _chargePercent;

        public override bool CanShootInAimingState => true;

        public override void OnAimStart(PlayerGun playerGunOwner)
        {
            _chargePercent = 0f;
            RaiseChargeUI(true);
        }

        public override void OnAimUpdate(PlayerGun playerGunOwner)
        {
            if (playerGunOwner.CurrentAmmo <= 0)
                return;

            float nextCharge = chargeTime <= 0f
                ? 1f
                : _chargePercent + Time.deltaTime / chargeTime;

            _chargePercent = Mathf.Clamp01(nextCharge);
            RaiseChargeUI(true);
        }

        public override void OnAimEnd(PlayerGun playerGunOwner)
        {
            RaiseChargeUI(false);
        }

        public override bool Shot(PlayerGun playerGunOwner)
        {
            if (playerGunOwner.CurrentAmmo <= 0) return false;
            if (Time.time < playerGunOwner.LastFireTime + FireInterval) return false;

            Ray ray = playerGunOwner.AimModule.GetAimRay();
            Vector3 lineStartPosition = playerGunOwner.LineEffectModule.transform.position;

            bool isHit = TryGetRocketHit(ray.origin, ray.direction, playerGunOwner, out Vector3 hitPoint, out Vector3 hitNormal, out float hitDistance);
            Vector3 endPosition = isHit ? hitPoint : ray.origin + ray.direction.normalized * MaxDistance;

            playerGunOwner.LineEffectModule.Shot(LineEffectDuration, lineStartPosition, endPosition);

            if (isHit)
            {
                Explode(playerGunOwner, hitPoint, hitNormal, GetChargedDamage(playerGunOwner));
                PlayExplosionEffect(hitPoint);
            }

            playerGunOwner.ShotSuccess(false);
            Bus<CameraRecoilEvent>.Raise(new CameraRecoilEvent(CameraShakePower, CameraShakeDuration, true, true));
            playerGunOwner.CameraImpulseModule?.GenerateImpulse();
            playerGunOwner.Owner.GetModule<GunCursorModule>()?.PlayScaleMotion();

            _chargePercent = 0f;
            RaiseChargeUI(false);
            return true;
        }

        public override bool ShotAI(PlayerGun playerGunOwner, AbstractEnemy target)
        {
            if (playerGunOwner.CurrentAmmo <= 0) return false;
            if (Time.time < playerGunOwner.LastFireTime + chargeTime) return false;
            if (target == null || !target.gameObject.activeSelf) return false;

            Vector3 lineStartPosition = playerGunOwner.LineEffectModule.transform.position;
            Vector3 direction = GetSpreadDirection((target.HitPos.position - lineStartPosition).normalized, AIRandomSpreadAngle);

            bool isHit = TryGetRocketHit(lineStartPosition, direction, playerGunOwner, out Vector3 hitPoint, out Vector3 hitNormal, out float hitDistance);
            Vector3 endPosition = isHit ? hitPoint : lineStartPosition + direction.normalized * MaxDistance;

            playerGunOwner.LineEffectModule.Shot(LineEffectDuration, lineStartPosition, endPosition);

            if (isHit)
            {
                Explode(playerGunOwner, hitPoint, hitNormal, Mathf.RoundToInt(playerGunOwner.CurrentDamage * maxDamageMultiplier));
                PlayExplosionEffect(hitPoint);
            }

            playerGunOwner.ShotSuccess();
            return true;
        }

        public override AbstractEnemy SelectAITarget(EnemyRegisterSo enemyRegisterSo, Transform myTransform)
        {
            return enemyRegisterSo.ClosestEnemy(myTransform);
        }

        private bool TryGetRocketHit(Vector3 origin, Vector3 direction, PlayerGun playerGunOwner, out Vector3 hitPoint, out Vector3 hitNormal, out float hitDistance)
        {
            hitPoint = origin + direction.normalized * MaxDistance;
            hitNormal = -direction.normalized;
            hitDistance = MaxDistance;

            RaycastHit[] hits = Physics.RaycastAll(origin, direction.normalized, MaxDistance, HitMask);
            if (hits.Length == 0)
                return false;

            Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            foreach (RaycastHit hit in hits)
            {
                if (!TryGetDamageable(hit.collider, out _))
                    continue;

                hitPoint = hit.point;
                hitNormal = hit.normal;
                hitDistance = hit.distance;
                SetActionData(playerGunOwner, hitPoint, hitNormal, hitDistance);
                return true;
            }

            return false;
        }

        private void Explode(PlayerGun playerGunOwner, Vector3 center, Vector3 hitNormal, int damage)
        {
            _damagedTargets.Clear();
            Collider[] colliders = Physics.OverlapSphere(center, explosionRadius, HitMask);

            foreach (Collider collider in colliders)
            {
                if (!TryGetDamageable(collider, out IDamageable damageable))
                    continue;

                if (!_damagedTargets.Add(damageable))
                    continue;

                Vector3 closestPoint = collider.ClosestPoint(center);
                Vector3 hitDirection = closestPoint - center;

                DamageData damageData = new DamageData
                {
                    Attacker = playerGunOwner.Owner,
                    Damage = damage,
                    HitPoint = closestPoint,
                    HitNormal = hitDirection.sqrMagnitude > 0.0001f ? -hitDirection.normalized : hitNormal,
                    HitDistance = hitDirection.magnitude,
                    DamageMultiplier = 1f
                };

                damageable.ApplyDamage(damageData);
            }
        }

        private bool TryGetDamageable(Collider collider, out IDamageable damageable)
        {
            if (collider.TryGetComponent(out damageable))
                return true;

            damageable = collider.GetComponentInParent<IDamageable>();
            return damageable != null;
        }

        private void SetActionData(PlayerGun playerGunOwner, Vector3 hitPoint, Vector3 hitNormal, float hitDistance)
        {
            if (playerGunOwner.ActionDataModule == null)
                return;

            playerGunOwner.ActionDataModule.HitPoint = hitPoint;
            playerGunOwner.ActionDataModule.HitNormal = hitNormal;
            playerGunOwner.ActionDataModule.HitDistance = hitDistance;
            playerGunOwner.ActionDataModule.Attacker = playerGunOwner.Owner;
        }

        private void PlayExplosionEffect(Vector3 position)
        {
            if (poolManagerSo == null || explosionEffect == null)
                return;

            ParticlePooling particle = poolManagerSo.Pop<ParticlePooling>(explosionEffect);
            if (particle == null)
                return;

            particle.PoolManagerSo = poolManagerSo;
            particle.Play(position);
        }

        private int GetChargedDamage(PlayerGun playerGunOwner)
        {
            float multiplier = Mathf.Lerp(minDamageMultiplier, maxDamageMultiplier, _chargePercent);
            return Mathf.RoundToInt(playerGunOwner.CurrentDamage * multiplier);
        }

        private void RaiseChargeUI(bool active)
        {
            float displayPercent = Mathf.Lerp(minDamageMultiplier, maxDamageMultiplier, _chargePercent) * 100f;
            Bus<SniperChargeUIEvent>.Raise(new SniperChargeUIEvent(_chargePercent, displayPercent, active));
        }
    }
}
