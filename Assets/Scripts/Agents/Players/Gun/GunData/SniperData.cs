using System;
using Agents.CombatSystem;
using Agents.Enemies;
using Agents.Module;
using CoreSystem.BusSystem;
using GameEvents.Camera;
using GameEvents.UI;
using UnityEngine;

namespace Agents.Players.Gun.GunData
{
    [Serializable]
    public class SniperData : GunData
    {
        [Header("Sniper")]
        [SerializeField] private float chargeTime = 1.2f;
        [SerializeField] private float minDamageMultiplier = 1f;
        [SerializeField] private float maxDamageMultiplier = 3f;

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
            int damage = GetChargedDamage(playerGunOwner);

            bool isHit = playerGunOwner.RayDamageCaster.RayCastDamage(ray.origin, ray.direction,
                new DamageData { Damage = damage, Attacker = playerGunOwner.Owner });

            Vector3 endPosition = isHit
                ? playerGunOwner.ActionDataModule.HitPoint
                : ray.origin + ray.direction.normalized * MaxDistance;

            playerGunOwner.LineEffectModule.Shot(LineEffectDuration, lineStartPosition, endPosition);
            playerGunOwner.ShotSuccess(false);

            Bus<CameraRecoilEvent>.Raise(new CameraRecoilEvent(CameraShakePower, CameraShakeDuration, false, true));
            playerGunOwner.Owner.GetModule<GunCursorModule>().PlayScaleMotion();
            _chargePercent = 0f;
            RaiseChargeUI(false);
            return true;
        }

        public override bool ShotAI(PlayerGun playerGunOwner, AbstractEnemy target)
        {
            if (playerGunOwner.CurrentAmmo <= 0) return false;
            if (Time.time < playerGunOwner.LastFireTime + FireInterval) return false;
            if (target == null || !target.gameObject.activeSelf) return false;

            Vector3 lineStartPosition = playerGunOwner.LineEffectModule.transform.position;
            Vector3 direction = GetSpreadDirection((target.HitPos.position - lineStartPosition).normalized, AIRandomSpreadAngle);
            int damage = Mathf.RoundToInt(playerGunOwner.CurrentDamage * maxDamageMultiplier);

            bool isHit = playerGunOwner.RayDamageCaster.RayCastDamage(lineStartPosition, direction,
                new DamageData { Damage = damage, Attacker = playerGunOwner.Owner });

            Vector3 endPosition = isHit
                ? playerGunOwner.ActionDataModule.HitPoint
                : lineStartPosition + direction.normalized * MaxDistance;

            playerGunOwner.LineEffectModule.Shot(LineEffectDuration, lineStartPosition, endPosition);
            playerGunOwner.ShotSuccess();
            return true;
        }

        public override AbstractEnemy SelectAITarget(EnemyRegisterSo enemyRegisterSo, Transform myTransform)
        {
            return enemyRegisterSo.FurthestEnemy(myTransform);
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
