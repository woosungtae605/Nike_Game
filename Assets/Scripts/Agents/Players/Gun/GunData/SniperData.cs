using System;
using Agents.Enemies;
using UnityEngine;

namespace Agents.Players.Gun.GunData
{
    [Serializable]
    public class SniperData : GunData
    {

        public override void Shot(PlayerGun playerGunOwner)
        {
            
        }

        public override Enemy AIShot(EnemyRegisterSo enemyRegisterSo, Transform myTransform)
        {
            return null;
        }
    }
}