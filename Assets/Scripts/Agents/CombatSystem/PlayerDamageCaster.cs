using System;
using Agents.Players;
using Agents.Players.Gun;
using Agents.Players.Gun.GunData;
using CoreSystem;
using CoreSystem.BusSystem;
using GameEvents;
using UnityEngine;

namespace Agents.CombatSystem
{
    public class PlayerDamageCaster : AbstractDamageCaster
    {
        [SerializeField] private UnityEngine.Camera mainCamera;
        private GunData _gunData;
        private Player _player;
        
        private Vector2 _mousePosition;
        
        public override void InitCaster(Agent owner)
        {
            base.InitCaster(owner);
            _player = owner as Player;
            Debug.Assert(_player != null, "_player is null");
            
            PlayerGun playerGun = owner.GetModule<PlayerGun>();
            Debug.Assert(playerGun != null, "playerGun don't have as module");
            
            _player.PlayerInputSo.OnMousePos += HandleMousePos;
            
            _gunData = playerGun.PlayerGunData.GunData;
            
            if (mainCamera == null)
                mainCamera = UnityEngine.Camera.main;
        }

        private void OnDisable()
        {
            if (_player != null && _player.PlayerInputSo != null)
                _player.PlayerInputSo.OnMousePos -= HandleMousePos;
        }

        private void HandleMousePos(Vector2 obj)
        {
            _mousePosition = obj;
        }

        public override bool RayCastDamage(Vector3 positionOffset, Vector3 directionOffset, DamageData damageData) // 초기값은 Vector3 positionOffset, Vector3 directionOffset이거 2개 Vector3.zero하면 된다.
        {
            Ray ray = mainCamera.ScreenPointToRay(_mousePosition);
            
            Vector3 origin = ray.origin + positionOffset;
            Vector3 direction = (ray.direction + directionOffset).normalized;
            
            if (!Physics.Raycast(origin, direction , out RaycastHit hitInfo, _gunData.MaxDistance, _gunData.HitMask))
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
            if (_gunData == null || mainCamera == null)
                return;

            Ray ray = mainCamera.ScreenPointToRay(_mousePosition);

            Vector3 origin = ray.origin;
            Vector3 direction = ray.direction.normalized;
            Vector3 end = origin + direction * _gunData.MaxDistance;

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(origin, end);
            Gizmos.DrawWireSphere(origin, 0.1f);
        }
    }
}