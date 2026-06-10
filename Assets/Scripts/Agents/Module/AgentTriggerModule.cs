using System;
using Module;
using UnityEngine;

namespace Agents.Module
{
    public class AgentTriggerModule : MonoBehaviour, IModule
    {
        public event Action OnAnimationEnd;
        public event Action OnDamageCast;
        
        public void Initialize(ModuleOwner owner)
        {
            //여기서는 안한다.    
        }
        
        private void AnimationEndTrigger() => OnAnimationEnd?.Invoke();
        private void DamageCastTrigger() => OnDamageCast?.Invoke();
    }
}
