using System;
using Agents.CombatSystem;
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
            if (playerGunOwner.RayDamageCaster.RayCastDamage(Vector3.zero,
                    Vector3.zero, new DamageData{ Damage = Damage, Attacker = playerGunOwner.Owner }))
            {
                playerGunOwner.Owner.GetModule<GunCursorImageModule>().PlayScaleMotion();
                playerGunOwner.ShotSuccess();
            }
        }
    }
}