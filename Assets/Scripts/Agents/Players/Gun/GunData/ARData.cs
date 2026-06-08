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
    public class ARData : GunData
    {
        public override bool Shot(PlayerGun playerGunOwner)
        {
            if (playerGunOwner.CurrentAmmo <= 0) return false;
            if (Time.time < playerGunOwner.LastFireTime + FireInterval) return false;
            
            Ray ray = playerGunOwner.AimModule.GetAimRay();
            Vector3 lineStartPosition = playerGunOwner.LineEffectModule.transform.position;

            if (playerGunOwner.RayDamageCaster.RayCastDamage(ray.origin, ray.direction,
                    new DamageData { Damage = Damage, Attacker = playerGunOwner.Owner }))
            {
                playerGunOwner.LineEffectModule.Shot(LineEffectDuration, lineStartPosition, playerGunOwner.ActionDataModule.HitPoint);   
            }
            else
            {
                playerGunOwner.LineEffectModule.Shot(LineEffectDuration, lineStartPosition, ray.origin + ray.direction.normalized * MaxDistance); 
            }
            
            playerGunOwner.ShotSuccess();
            
            Bus<CameraRecoilEvent>.Raise(new CameraRecoilEvent(CameraShakePower, CameraShakeDuration, false, true));
            playerGunOwner.Owner.GetModule<GunCursorModule>().PlayScaleMotion();
            return true;
        }

        public override bool ShotAI(PlayerGun playerGunOwner, Enemy target)
        {
            if (playerGunOwner.CurrentAmmo <= 0) return false;
            if (Time.time < playerGunOwner.LastFireTime + FireInterval) return false;
            if (target == null || !target.gameObject.activeSelf) return false;

            Vector3 lineStartPosition = playerGunOwner.LineEffectModule.transform.position;
            Vector3 direction = GetSpreadDirection((target.hitPos.position - lineStartPosition).normalized, AIRandomSpreadAngle);

            bool isHit = playerGunOwner.RayDamageCaster.RayCastDamage(lineStartPosition, direction,
                new DamageData { Damage = Damage, Attacker = playerGunOwner.Owner });

            Vector3 endPosition = isHit
                ? playerGunOwner.ActionDataModule.HitPoint
                : lineStartPosition + direction * MaxDistance;

            playerGunOwner.LineEffectModule.Shot(LineEffectDuration, lineStartPosition, endPosition);
            playerGunOwner.ShotSuccess();
            return true;
        }


        public override Enemy SelectAITarget(EnemyRegisterSo enemyRegisterSo, Transform myTransform)
        {
            return enemyRegisterSo.ClosestEnemy(myTransform);
        }
    


}
}