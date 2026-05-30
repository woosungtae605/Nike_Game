using Agents.Players.Gun;
using Agents.Players.Gun.GunData;
using Module;
using UnityEngine;

namespace Agents.CombatSystem
{
    public class DamageCaster : AbstractDamageCaster
    {
        private GunData _gunData;
        
        public override void InitCaster(Agent owner)
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
            
            ApplyDamage(hitInfo, damageData);
        }

        public override void SphereCastDamage(Vector3 position, Vector3 direction, DamageData damageData, float radius)
        {
            RaycastHit[] hits = Physics.SphereCastAll(
                position, radius, direction, _gunData.MaxDistance, _gunData.HitMask);

            foreach (RaycastHit hitInfo in hits)
            {
                ApplyDamage(hitInfo, damageData);
            }
        }

        public override void BoxCastDamage(Vector3 position, Vector3 direction, DamageData damageData, Vector3 halfExtents)
        {
            RaycastHit[] hits = Physics.BoxCastAll( 
                position, halfExtents, direction, transform.rotation, _gunData.MaxDistance, _gunData.HitMask);

            foreach (RaycastHit hitInfo in hits)
            {
                ApplyDamage(hitInfo, damageData);
            }
        }

        private void ApplyDamage(RaycastHit hitInfo, DamageData damageData)
        {
            damageData.HitPoint = hitInfo.point;
            damageData.HitNormal = hitInfo.normal;

            if (!hitInfo.collider.TryGetComponent(out IDamageable damageable))
                return;
            
            damageable.ApplyDamage(damageData);
        }
    }
}