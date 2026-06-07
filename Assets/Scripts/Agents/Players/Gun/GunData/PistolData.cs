using System;
using Agents.Enemies;
using UnityEngine;

namespace Agents.Players.Gun.GunData
{
    [Serializable]
    public class PistolData : GunData
    {

        public override bool Shot(PlayerGun playerGunOwner)
        {
            return true;
        }

        public override Enemy SelectAITarget(EnemyRegisterSo enemyRegisterSo, Transform myTransform)
        {
            return null;
        }
    }
}