using Agents.Players.Gun;
using Agents.Players.Gun.GunData;
using Module;
using UnityEngine;

namespace Agents.CombatSystem
{
    public class RayDamageCaster : AbstractDamageCaster
    {
        private GunData _gunData;
        
        public override void InitCaster(ModuleOwner owner)
        {
            base.InitCaster(owner);
            PlayerGun playerGun = owner.GetModule<PlayerGun>();
            _gunData = playerGun.PlayerGunData.GunData;
        }
        
        public override void CastDamage(Vector3 position, Vector3 direction, DamageData damageData)
        {
            
        }
    }
}