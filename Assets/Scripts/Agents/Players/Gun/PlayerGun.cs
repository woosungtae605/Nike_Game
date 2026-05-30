using Agents.CombatSystem;
using Module;
using UnityEngine;

namespace Agents.Players.Gun
{
    public class PlayerGun : MonoBehaviour, IModule
    {
        [field: SerializeField] public PlayerGunDataSO PlayerGunData { get; private set; }
        
        private Agent _owner;
        
        private int _currentAmmo;
        private float _lastFireTime;
        
        private AbstractDamageCaster _rayDamageCaster;
        
        public void Initialize(ModuleOwner owner)
        {
            _owner = owner as Agent;
            Debug.Assert(PlayerGunData != null, "PlayerGunData is null");
            _currentAmmo = PlayerGunData.GunData.MaxAmmo;

            _rayDamageCaster = GetComponentInChildren<AbstractDamageCaster>();
            Debug.Assert(_rayDamageCaster != null, "Don't have AbstractDamageCaster as children");
        }
        public bool TryFire()
        {
            if (_currentAmmo <= 0) return false;
            if (Time.time < _lastFireTime + PlayerGunData.GunData.FireInterval) return false;

            _lastFireTime = Time.time;
            _currentAmmo--;
            PlayerGunData.GunData.Shot(this);
            return true;
        }

        public void RayDamageCast(Vector3 direction)
        {
            
        }
    }
}