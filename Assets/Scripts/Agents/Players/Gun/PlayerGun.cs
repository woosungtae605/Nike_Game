using Agents.CombatSystem;
using Module;
using UnityEngine;

namespace Agents.Players.Gun
{
    public class PlayerGun : MonoBehaviour, IModule
    {
        [field: SerializeField] public PlayerGunDataSO PlayerGunData { get; private set; }

        public GunData.GunData GunData => PlayerGunData.GunData;
        
        private Player _owner;
        
        public Player Owner => _owner;
        
        private int _currentAmmo;
        public int CurrentAmmo => _currentAmmo;
        
        private float _lastFireTime;
        
        public AbstractDamageCaster RayDamageCaster { get; private set; }
        
        public void Initialize(ModuleOwner owner)
        {
            _owner = owner as Player;
            Debug.Assert(PlayerGunData != null, "PlayerGunData is null");
            _currentAmmo = PlayerGunData.GunData.MaxAmmo;

            RayDamageCaster = GetComponentInChildren<AbstractDamageCaster>();
            Debug.Assert(RayDamageCaster != null, "Don't have AbstractDamageCaster as children");
            RayDamageCaster.InitCaster(_owner);
        }
        public bool TryFire()
        {
            if (_currentAmmo <= 0) return false;
            if (Time.time < _lastFireTime + PlayerGunData.GunData.FireInterval) return false;
            
            PlayerGunData.GunData.Shot(this);
            return true;
        }

        public void ShotSuccess()
        {
            _lastFireTime = Time.time;
            _currentAmmo--;
        }

        public void Reload()
        {
            _currentAmmo = PlayerGunData.GunData.MaxAmmo;
        }
    }
}