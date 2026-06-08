using System;
using Agents.Enemies;
using UnityEngine;

namespace Agents.Players.Gun.GunData
{
    [Serializable]
    public class RocketLauncherData : GunData
    {

        public override bool Shot(PlayerGun playerGunOwner)
        {
            return true;
        }

        public override bool ShotAI(PlayerGun playerGunOwner, Enemy target)
        {
            throw new NotImplementedException();
        }

        public override Enemy SelectAITarget(EnemyRegisterSo enemyRegisterSo, Transform myTransform)
        {
            return null;
        }
    }
}