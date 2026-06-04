using System;
using Agents.CombatSystem;
using Agents.Enemies;
using Agents.Module;
using CoreSystem.BusSystem;
using GameEvents.Camera;
using GameEvents.UI;
using UI.BattleUI.NikkeShotUI;
using UnityEngine;

namespace Agents.Players.Gun.GunData
{
    [Serializable]
    public class ARData : GunData
    {
        [Header("ARData")]
        [SerializeField] public float ar;

        public override void Shot(PlayerGun playerGunOwner)
        {
            Ray ray = playerGunOwner.AimModule.GetAimRay();

            if (playerGunOwner.RayDamageCaster.RayCastDamage(ray.origin, ray.direction,
                    new DamageData { Damage = Damage, Attacker = playerGunOwner.Owner }))
            {
                Bus<HitCursorUIEvent>.Raise(new HitCursorUIEvent(false));   
            }
            Bus<CameraRecoilEvent>.Raise(new CameraRecoilEvent(CameraShakePower, CameraShakeDuration, false, true));
        }

        public override Enemy SelectAITarget(EnemyRegisterSo enemyRegisterSo, Transform myTransform)
        {
            return enemyRegisterSo.ClosestEnemy(myTransform);
        }
    }
}