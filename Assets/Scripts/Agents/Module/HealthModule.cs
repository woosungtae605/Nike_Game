using System;
using Module;
using Unity.Cinemachine;
using UnityEngine;

namespace Agents.Module
{
    public class HealthModule : MonoBehaviour, IModule, IBar
    {
        [SerializeField] private int maxHealth;
        [SerializeField] private int currentHealth;
        
        private ModuleOwner _owner;
        
        
        public int CurrentValue => currentHealth;
        public int MaxValue => maxHealth;
        public event Action<int, int> OnChanged;

        public event Action OnDeath;
        
        public void Initialize(ModuleOwner owner)
        {
            _owner = owner;
            currentHealth = maxHealth; 
            OnChanged?.Invoke(currentHealth, maxHealth);
        }

        public void ChangeHealth(int amount)
        {
            maxHealth = amount;
            currentHealth = maxHealth;
            OnChanged?.Invoke(currentHealth, maxHealth);
        }

        public void ApplyDamage(int damageAmount)
        {
            currentHealth -= damageAmount;
            currentHealth = Mathf.Max(currentHealth, 0);

            OnChanged?.Invoke(currentHealth, maxHealth);
            if (currentHealth <= 0)
            {
                OnDeath?.Invoke();
            }
        }
    }
}