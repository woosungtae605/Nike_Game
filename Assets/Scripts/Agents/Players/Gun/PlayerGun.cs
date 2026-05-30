using Module;
using UnityEngine;

namespace Agents.Players.Gun
{
    public class PlayerGun : MonoBehaviour, IModule
    {
        private Agent _owner;
        [field: SerializeField] public PlayerGunDataSO PlayerGunData { get; private set; }
        private int _currentAmmo;
        private float _lastFireTime;
        public void Initialize(ModuleOwner owner)
        {
            _owner = owner as Agent;
            _currentAmmo = PlayerGunData.GunData.MaxAmmo;
            Debug.Assert(PlayerGunData != null, "PlayerGunData is null");
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
    }
}