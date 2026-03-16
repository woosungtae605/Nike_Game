using System;
using Agents;
using Systems.AnimationSystems;
using UnityEngine;

namespace FSM
{
    public abstract class AgentState : MonoBehaviour
    {
        protected Agent _owner;
        protected int _clipHash;
        protected bool _isTriggerCall;
        
        protected IRenderer  _renderer;

        public AgentState(Agent owner, AnimParamSO stateParam)
        {
            _owner = owner;
            _clipHash = stateParam.ParamHash;
            _renderer = GetComponent<IRenderer>();
        }

        public virtual void Update()
        {
            
        }
        
        public virtual void Enter()
        {
            _renderer.PlayClip(_clipHash);
            _isTriggerCall = false;
        }
        
        public virtual void Exit(){}
 
        public virtual void AnimationEndTrigger() => _isTriggerCall = true;
    }
}