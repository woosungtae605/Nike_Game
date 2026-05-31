using System;
using Agents.CombatSystem;
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
            playerGunOwner.RayDamageCaster.RayCastDamage(playerGunOwner.Owner.transform.position, playerGunOwner.Owner.transform.forward, new DamageData
            {
                Damage = Damage,
                Attacker = playerGunOwner.Owner
            });
        }
    }
}