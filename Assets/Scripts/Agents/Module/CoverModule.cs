using System;
using Agents.CombatSystem;
using Module;
using UnityEngine;

namespace Agents.Module
{
    public class CoverModule : MonoBehaviour, IModule, IBar
    {
        [SerializeField] private int maxCoverHp;

        private int _currentCoverHp;
        
        private ModuleOwner _owner;
        
        public int CurrentValue => _currentCoverHp;
        public int MaxValue => maxCoverHp;
        public event Action<int, int> OnChanged;
        
        public bool IsHide { get; private set; }
        public bool IsCoverBroken => _currentCoverHp <= 0;

        public event Action<bool> OnCoverValueChange;
        
        public void Initialize(ModuleOwner owner)
        {
            _owner = owner;
            _currentCoverHp = maxCoverHp;
            OnChanged?.Invoke(_currentCoverHp, maxCoverHp);
        }
        
        public void SetHide(bool value)
        {
            IsHide = value;
            OnCoverValueChange?.Invoke(value);
        }

        public void ApplyCoverDamage(DamageData damageData)
        {
            if (IsCoverBroken)
                return;

            _currentCoverHp -= damageData.Damage;
            OnChanged?.Invoke(_currentCoverHp, maxCoverHp);

            if (_currentCoverHp <= 0)
            {
                _currentCoverHp = 0;
                IsHide = false;
            }
        }
    }
}