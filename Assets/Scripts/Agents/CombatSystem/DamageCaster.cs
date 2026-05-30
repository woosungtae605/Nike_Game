using Agents.Players.Gun;
using Agents.Players.Gun.GunData;
using Module;
using UnityEngine;

namespace Agents.CombatSystem
{
    public class DamageCaster : AbstractDamageCaster
    {
        private GunData _gunData;
        
        public override void InitCaster(ModuleOwner owner)
        {
            base.InitCaster(owner);
            PlayerGun playerGun = owner.GetModule<PlayerGun>();
            Debug.Assert(playerGun != null, "playerGun don't have as module");
            _gunData = playerGun.PlayerGunData.GunData;
        }
        
        public override void RayCastDamage(Vector3 position, Vector3 direction, DamageData damageData)
        {
            if (!Physics.Raycast(position, direction, out RaycastHit hitInfo, _gunData.MaxDistance, _gunData.HitMask))
                return;

            damageData.HitPoint = hitInfo.point;
            damageData.HitNormal = hitInfo.normal;

            if (hitInfo.collider.TryGetComponent(out IDamageable damageable))
            {
                damageable.ApplyDamage(damageData);
                return;
            }

            damageable = hitInfo.collider.GetComponentInParent<IDamageable>();
            damageable?.ApplyDamage(damageData);
        }
    }
}