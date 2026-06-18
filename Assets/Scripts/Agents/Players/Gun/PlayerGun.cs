using Agents.CombatSystem;
using Agents.Enemies;
using Agents.Module;
using CoreSystem.BusSystem;
using GameEvents.UI;
using Module;
using UnityEngine;

namespace Agents.Players.Gun
{
    public class PlayerGun : MonoBehaviour, IModule, IAfterInitModule
    {
        public PlayerGunDataSO PlayerGunData { get; private set; }

        public GunData.GunData GunData => PlayerGunData.GunData;
        
        private Player _owner;
        
        public Player Owner => _owner;
        
        private int _currentAmmo;
        public int CurrentAmmo => _currentAmmo;
        public int CurrentDamage => Owner != null && Owner.AttackDamage > 0 ? Owner.AttackDamage : GunData.Damage;
        
        private float _lastFireTime;
        public float LastFireTime => _lastFireTime;
        
        public PlayerAimModule AimModule { get; private set; }
        public AbstractDamageCaster RayDamageCaster { get; private set; }
        public GunLineEffectModule  LineEffectModule { get; private set; }
        public ActionDataModule  ActionDataModule { get; private set; }
        
        public void Initialize(ModuleOwner owner)
        {
            _owner = owner as Player;
            Debug.Assert(_owner != null, "owner is not Player");

            PlayerGunData = _owner.PlayerData.PlayerGunData;
            Debug.Assert(PlayerGunData != null, "PlayerGunData is null");
            _currentAmmo = PlayerGunData.GunData.MaxAmmo;

            RayDamageCaster = GetComponentInChildren<AbstractDamageCaster>();
            Debug.Assert(RayDamageCaster != null, "Don't have AbstractDamageCaster as children");
            RayDamageCaster.InitCaster(_owner);
        }
        
        public void AfterInit()
        {
            AimModule = Owner.GetModule<PlayerAimModule>();
            LineEffectModule = Owner.GetModule<GunLineEffectModule>();
            ActionDataModule = Owner.GetModule<ActionDataModule>();
        }
        
        public bool TryFirePlayer()
        {
            if (!_owner.IsControl)
                return false;

            if (!PlayerGunData.GunData.Shot(this))
                return false;
            
            return true;
        }
        
        public bool TryFireAI(AbstractEnemy target)
        {
            return PlayerGunData.GunData.ShotAI(this, target);
        }
        
        public AbstractEnemy GetAITarget(EnemyRegisterSo enemyRegisterSo)
        {
            if (enemyRegisterSo == null)
                return null;

            return GunData.SelectAITarget(enemyRegisterSo, Owner.transform);
        }

        public void ShotSuccess(bool showAmmoUI = true)
        {
            _lastFireTime = Time.time;
            _currentAmmo--;
            
            if(_owner.IsControl && showAmmoUI)
                Bus<GunAmmoUIActiveEvent>.Raise(new GunAmmoUIActiveEvent(CurrentAmmo, GunData.MaxAmmo, true));
        }

        public void Reload()
        {
            _currentAmmo = PlayerGunData.GunData.MaxAmmo;
        }
    }
}
