using UnityEngine;

namespace Systems.AnimationSystems
{
    public interface IRenderer
    {
        Animator Animator { get; }
        void PlayClip(int clipHash, float crossFadeDuration, float normalizedTime = 0, int layerIndex = 0);
        void SetBool(AnimParamSO param, bool value);
        void SetFloat(AnimParamSO param, float value);
        void SetFloat(AnimParamSO param, float value, float dampTime, float deltaTime);
        void SetInt(AnimParamSO param, int value);
        void SetTrigger(AnimParamSO param);
    }
}