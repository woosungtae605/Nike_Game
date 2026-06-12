using System;
using Agents.Module;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

namespace Agents.Enemies.Action
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "AnimationEnd", story: "[Enemy] animation end", category: "Action", id: "c2e12e3d079efe9d5e786b131756ead7")]
    public partial class AnimationEndAction : Unity.Behavior.Action
    {
        [SerializeReference] public BlackboardVariable<AbstractEnemy> Enemy;

        private AgentTriggerModule _triggerModule;
        
        private bool _animationEnd = false;
        protected override Status OnStart()
        {
            if(Enemy == null || Enemy.Value == null)
                return Status.Failure;
            
            _animationEnd = false;

            _triggerModule = Enemy.Value.Trigger;
            
            if(_triggerModule == null)
                return Status.Failure;

            _triggerModule.OnAnimationEnd -= HandleAnimationEnd;
            _triggerModule.OnAnimationEnd += HandleAnimationEnd;

            return Status.Running;
        }

        private void HandleAnimationEnd()
        {
            _animationEnd = true;
        }

        protected override Status OnUpdate()
        {
            return _animationEnd ? Status.Success : Status.Running;
        }
        
        protected override void OnEnd()
        {
            if (_triggerModule != null)
                _triggerModule.OnAnimationEnd -= HandleAnimationEnd;
        }
    }
}

