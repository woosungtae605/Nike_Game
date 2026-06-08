using Module;
using Systems.AnimationSystems;
using UnityEngine;

namespace Agents.Module
{

    [RequireComponent(typeof(Animator))]
    public class RendererModule : MonoBehaviour, IModule, IRenderer
    {
        protected Agent _owner;
        public Animator Animator { get; private set; }
        
        public virtual void Initialize(ModuleOwner owner)
        {
            _owner = owner as Agent;
            Animator = GetComponent<Animator>();
        }

        public void PlayClip(int clipHash, float crossFadeDuration, float normalizedTime = 0, int layerIndex = 0)
        {
            Animator.CrossFadeInFixedTime(clipHash, crossFadeDuration, layerIndex, normalizedTime);
        }

        public void SetBool(AnimParamSO param, bool value)
            => Animator.SetBool(param.ParamHash, value);
        public void SetFloat(AnimParamSO param, float value)
            => Animator.SetFloat(param.ParamHash, value);
        public void SetInt(AnimParamSO param, int value)
            => Animator.SetInteger(param.ParamHash, value);
        public void SetTrigger(AnimParamSO param)
            => Animator.SetTrigger(param.ParamHash);
    }
}