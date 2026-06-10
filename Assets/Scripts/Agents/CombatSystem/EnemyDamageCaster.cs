using System;
using Agents.Enemies;
using UnityEngine;

namespace Agents.CombatSystem
{
    public class EnemyDamageCaster : AbstractDamageCaster
    {
        private const float DefaultMaxDistance = 60f;
        private const int DefaultHitMask = 1 << 7;

        public override bool RayCastDamage(Vector3 origin, Vector3 direction, DamageData damageData)
        {
            return RayCastDamage(origin, direction, damageData, DefaultMaxDistance, DefaultHitMask);
        }

        public bool RayCastDamage(Vector3 origin, Vector3 direction, DamageData damageData, float maxDistance, LayerMask hitMask)
        {
            RaycastHit[] hits = Physics.RaycastAll(origin, direction.normalized, maxDistance, hitMask);
            if (hits.Length == 0)
                return false;

            Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            foreach (RaycastHit hitInfo in hits)
            {
                if (ApplyDamage(hitInfo, damageData))
                    return true;
            }

            return false;
        }

        public override void SphereCastDamage(Vector3 position, Vector3 direction, DamageData damageData, float radius)
        {
            SphereCastDamage(position, direction, damageData, radius, DefaultMaxDistance, DefaultHitMask);
        }

        public void SphereCastDamage(Vector3 position, Vector3 direction, DamageData damageData, float radius, float maxDistance, LayerMask hitMask)
        {
            RaycastHit[] hits = Physics.SphereCastAll(position, radius, direction.normalized, maxDistance, hitMask);
            Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            foreach (RaycastHit hitInfo in hits)
            {
                ApplyDamage(hitInfo, damageData);
            }
        }

        public override void BoxCastDamage(Vector3 position, Vector3 direction, DamageData damageData, Vector3 halfExtents)
        {
            BoxCastDamage(position, direction, damageData, halfExtents, DefaultMaxDistance, DefaultHitMask);
        }

        public void BoxCastDamage(Vector3 position, Vector3 direction, DamageData damageData, Vector3 halfExtents, float maxDistance, LayerMask hitMask)
        {
            RaycastHit[] hits = Physics.BoxCastAll(position, halfExtents, direction.normalized, transform.rotation, maxDistance, hitMask);
            Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            foreach (RaycastHit hitInfo in hits)
            {
                ApplyDamage(hitInfo, damageData);
            }
        }

        private bool ApplyDamage(RaycastHit hitInfo, DamageData damageData)
        {
            if (CasterOwner != null && hitInfo.collider.GetComponentInParent<Agent>() == CasterOwner)
                return false;

            damageData.HitPoint = hitInfo.point;
            damageData.HitNormal = hitInfo.normal;
            damageData.HitDistance = hitInfo.distance;

            if (hitInfo.collider.TryGetComponent(out IDamageable damageable))
            {
                damageable.ApplyDamage(damageData);
                return true;
            }

            damageable = hitInfo.collider.GetComponentInParent<IDamageable>();
            if (damageable == null)
                return false;

            damageable.ApplyDamage(damageData);
            return true;
        }
    }
}
