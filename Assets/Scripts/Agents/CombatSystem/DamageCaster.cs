using Agents.Players.Gun;
using Agents.Players.Gun.GunData;
using CoreSystem;
using GameEvents;
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
        
        public override bool RayCastDamage(Vector3 position, Vector3 direction, DamageData damageData)
        {
            if (!Physics.Raycast(position, direction, out RaycastHit hitInfo, _gunData.MaxDistance, _gunData.HitMask))
                return false;
            
            ApplyDamage(hitInfo, damageData);
            return true;
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
            damageData.HitDistance = hitInfo.distance;

            if (!hitInfo.collider.TryGetComponent(out IDamageable damageable))
                return;
            
            damageable.ApplyDamage(damageData);
            Bus<CameraRecoilEvent>.Raise(new CameraRecoilEvent(_gunData.CameraShakePower, _gunData.CameraShakeDuration, false, true));
        }
        
        private void OnDrawGizmos()
        {
            if (_gunData == null)
                return;

            Vector3 start = transform.position;
            Vector3 direction = transform.forward;
            Vector3 end = start + direction * _gunData.MaxDistance;

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(start, end);

            Gizmos.DrawWireSphere(start, 0.1f);
        }
    }
}