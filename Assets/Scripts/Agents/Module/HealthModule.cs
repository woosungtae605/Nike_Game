using System;
using Module;
using UnityEngine;

namespace Agents.Module
{
    public class HealthModule : MonoBehaviour, IModule
    {
        [SerializeField] private int maxHealth;
        [SerializeField] private int currentHealth;
        
        private ModuleOwner _owner;

        public event Action OnDeath;
        
        public void Initialize(ModuleOwner owner)
        {
            _owner = owner;
            currentHealth = maxHealth; 
        }

        public void ChangeHealth(int amount)
        {
            maxHealth = amount;
            currentHealth = maxHealth;
        }

        public void ApplyDamage(int damageAmount)
        {
            currentHealth -= damageAmount;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                OnDeath?.Invoke();
            }
        }
    }
}