using Agents.CombatSystem;
using Module;
using UnityEngine;

namespace Agents.Module
{
    public class CoverModule : MonoBehaviour, IModule
    {
        [SerializeField] private int maxCoverHp;

        private int _currentCoverHp;
        private ModuleOwner _owner;
        
        public bool IsHide { get; private set; }
        public bool IsCoverBroken => _currentCoverHp <= 0;
        
        public void Initialize(ModuleOwner owner)
        {
            _owner = owner;
            _currentCoverHp = maxCoverHp;
        }
        
        public void SetHide(bool value)
        {
            IsHide = value;
        }

        public void ApplyCoverDamage(DamageData damageData)
        {
            if (IsCoverBroken)
                return;

            _currentCoverHp -= damageData.Damage;

            if (_currentCoverHp <= 0)
            {
                _currentCoverHp = 0;
                IsHide = false;
            }
        }
    }
}