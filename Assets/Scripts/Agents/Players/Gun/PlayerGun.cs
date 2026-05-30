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
        }

        public void Fire()
        {
            PlayerGunData.GunData.Shot(this);
        }
    }
}