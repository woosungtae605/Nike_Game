using System;
using Agents.Enemies;
using UnityEngine;

namespace Agents.Players.Gun.GunData
{
    [Serializable]
    public class RocketLauncherData : GunData
    {

        public override void Shot(PlayerGun playerGunOwner)
        {
            
        }

        public override Enemy SelectAITarget(EnemyRegisterSo enemyRegisterSo, Transform myTransform)
        {
            return null;
        }
    }
}