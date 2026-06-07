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
        [Header("ARData")]
        [SerializeField] public float ar;

        public override bool Shot(PlayerGun playerGunOwner)
        {
            if (playerGunOwner.CurrentAmmo <= 0) return false;
            if (Time.time < playerGunOwner.LastFireTime + FireInterval) return false;
            Ray ray = playerGunOwner.AimModule.GetAimRay();

            playerGunOwner.RayDamageCaster.RayCastDamage(ray.origin, ray.direction,
                    new DamageData { Damage = Damage, Attacker = playerGunOwner.Owner });
            
            playerGunOwner.ShotSuccess();
            Bus<CameraRecoilEvent>.Raise(new CameraRecoilEvent(CameraShakePower, CameraShakeDuration, false, true));
            playerGunOwner.Owner.GetModule<GunCursorModule>().PlayScaleMotion();
            return true;
        }

        public override Enemy SelectAITarget(EnemyRegisterSo enemyRegisterSo, Transform myTransform)
        {
            return enemyRegisterSo.ClosestEnemy(myTransform);
        }
    }
}