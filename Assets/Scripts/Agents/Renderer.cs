using Module;
using Systems.AnimationSystems;
using UnityEngine;

namespace Agents
{

    [RequireComponent(typeof(Animator))]
    public class Renderer : MonoBehaviour, IModule, IRenderer
    {
        private Agent _owner;
        private Animator _animator;
        
        public void Initialize(ModuleOwner owner)
        {
            _owner = owner as Agent;
            _animator = GetComponent<Animator>();
        }
        
        public void PlayClip(int clipHash, int layer = -1, float normalizedTime = 0)
            => _animator.Play(clipHash, layer, normalizedTime);

        public void SetBool(AnimParamSO param, bool value)
            => _animator.SetBool(param.ParamHash, value);
        public void SetFloat(AnimParamSO param, float value)
            => _animator.SetFloat(param.ParamHash, value);
        public void SetInt(AnimParamSO param, int value)
            => _animator.SetInteger(param.ParamHash, value);
        public void SetTrigger(AnimParamSO param)
            => _animator.SetTrigger(param.ParamHash);
    }
}