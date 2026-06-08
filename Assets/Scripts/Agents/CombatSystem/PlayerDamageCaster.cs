using System;
using Agents.Module;
using Agents.Players;
using Agents.Players.Gun;
using Agents.Players.Gun.GunData;
using CoreSystem;
using CoreSystem.BusSystem;
using GameEvents;
using GameEvents.Camera;
using UnityEngine;

namespace Agents.CombatSystem
{
    public class PlayerDamageCaster : AbstractDamageCaster
    {
        private GunData _gunData;
        private Player _player;        private ActionDataModule _actionDataModule;

        
        public override void InitCaster(Agent owner)
        {
            base.InitCaster(owner);
            _player = owner as Player;
            Debug.Assert(_player != null, "_player is null");
            
            PlayerGun playerGun = owner.GetModule<PlayerGun>();
            Debug.Assert(playerGun != null, "playerGun don't have as module");
            
            _gunData = playerGun.PlayerGunData.GunData;
        }

        public override bool RayCastDamage(Vector3 origin, Vector3 direction, DamageData damageData) // 초기값은 Vector3 positionOffset, Vector3 directionOffset이거 2개 Vector3.zero하면 된다.
        {
            if (!Physics.Raycast(origin, direction.normalized, out RaycastHit hitInfo, _gunData.MaxDistance, _gunData.HitMask))
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

            if (_actionDataModule == null)
                _actionDataModule = _player.GetModule<ActionDataModule>();

            if (_actionDataModule != null)
            {
                _actionDataModule.HitPoint = damageData.HitPoint;
                _actionDataModule.HitNormal = damageData.HitNormal;
                _actionDataModule.HitDistance = damageData.HitDistance;
                _actionDataModule.Attacker = damageData.Attacker;
            }

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