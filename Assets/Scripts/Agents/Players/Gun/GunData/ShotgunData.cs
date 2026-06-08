using System;
using Agents.CombatSystem;
using Agents.Enemies;
using Agents.Module;
using CoreSystem.BusSystem;
using GameEvents.Camera;
using UnityEngine;

namespace Agents.Players.Gun.GunData
{
    [Serializable]
    public class ShotgunData : GunData
    {
        [Header("ShotgunData")] 
        [SerializeField] private int shotgunShootAmmoCount;
        [SerializeField] private float spreadAngle;
        
        public override bool Shot(PlayerGun playerGunOwner)
        {
            if (playerGunOwner.CurrentAmmo <= 0) return false;
            if (Time.time < playerGunOwner.LastFireTime + FireInterval) return false;
            Ray ray = playerGunOwner.AimModule.GetAimRay();
            Vector3 lineStartPosition = playerGunOwner.LineEffectModule.transform.position;
            
            for (int i = 0; i < shotgunShootAmmoCount; i++)
            {
                Vector3 spreadDirection = GetSpreadDirection(ray.direction, spreadAngle);
                bool isHit = playerGunOwner.RayDamageCaster.RayCastDamage(ray.origin, spreadDirection,
                    new DamageData { Damage = Damage, Attacker = playerGunOwner.Owner });
                
                Vector3 endPosition = isHit
                    ? playerGunOwner.ActionDataModule.HitPoint
                    : ray.origin + spreadDirection.normalized * MaxDistance;

                playerGunOwner.LineEffectModule.Shot(LineEffectDuration, lineStartPosition, endPosition);
            }
            
            playerGunOwner.ShotSuccess();
            Bus<CameraRecoilEvent>.Raise(new CameraRecoilEvent(CameraShakePower, CameraShakeDuration, true, true));
            playerGunOwner.Owner.GetModule<GunCursorModule>().PlayScaleMotion();
            return true;
        }

        public override bool ShotAI(PlayerGun playerGunOwner, Enemy target)
        {
            if (playerGunOwner.CurrentAmmo <= 0) return false;
            if (Time.time < playerGunOwner.LastFireTime + FireInterval) return false;
            if (target == null || !target.gameObject.activeSelf) return false;

            Vector3 lineStartPosition = playerGunOwner.LineEffectModule.transform.position;
            Vector3 baseDirection = (target.hitPos.position - lineStartPosition).normalized;

            for (int i = 0; i < shotgunShootAmmoCount; i++)
            {
                Vector3 spreadDirection = GetSpreadDirection(baseDirection, spreadAngle);
                bool isHit = playerGunOwner.RayDamageCaster.RayCastDamage(lineStartPosition, spreadDirection,
                    new DamageData { Damage = Damage, Attacker = playerGunOwner.Owner });

                Vector3 endPosition = isHit
                    ? playerGunOwner.ActionDataModule.HitPoint
                    : lineStartPosition + spreadDirection.normalized * MaxDistance;

                playerGunOwner.LineEffectModule.Shot(LineEffectDuration, lineStartPosition, endPosition);
            }

            playerGunOwner.ShotSuccess();
            return true;
        }


        public override Enemy SelectAITarget(EnemyRegisterSo enemyRegisterSo, Transform myTransform)
        {
            return enemyRegisterSo.ClosestEnemy(myTransform);
        }
        
        private Vector3 GetSpreadDirection(Vector3 baseDirection, float angle)
        {
            float halfAngle = angle * 0.5f;
            Vector3 randomOffset = new Vector3(UnityEngine.Random.Range(-halfAngle, halfAngle), UnityEngine.Random.Range(-halfAngle, halfAngle), 0f);
            return Quaternion.Euler(randomOffset) * baseDirection;
        }
    }
}