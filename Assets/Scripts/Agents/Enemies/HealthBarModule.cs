using System;
using Agents.Module;
using Module;
using UnityEngine;

namespace Agents.Enemies
{
    public class HealthBarModule : MonoBehaviour, IModule, IAfterInitModule
    {
        [SerializeField] private GameObjectBar healthBar;

        private Agent _agent;
        private HealthModule _health;
        
        public void Initialize(ModuleOwner owner)
        {
            _agent = owner as Agent;
            _health = owner.GetModule<HealthModule>();
        }

        public void AfterInit()
        {
            _health.OnChanged += healthBar.SetValue;
        }

        private void OnDestroy()
        {
            if (_health != null && healthBar != null)
                _health.OnChanged -= healthBar.SetValue;
        }
    }
}