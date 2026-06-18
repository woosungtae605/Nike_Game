using System;
using Module;
using Spine;
using Spine.Unity;
using Systems.AnimationSystems;
using UnityEngine;

namespace Agents.Module
{
    [RequireComponent(typeof(SkeletonAnimation))]
    [RequireComponent(typeof(SkeletonRenderer))]
    public class SpineRendererModule : MonoBehaviour, IModule, IRenderer
    {
        [SerializeField] private SkeletonAnimation skeletonAnimation;
        [SerializeField] private SkeletonRenderer skeletonRenderer;
        [SerializeField] private SpineAnimationMap[] animationMaps;
        [SerializeField] private string defaultAnimation = "idle";
        [SerializeField] private SkeletonDataAsset defaultSkeletonDataAsset;
        [SerializeField] private bool defaultLoop = true;

        private Agent _owner;
        private TrackEntry _currentTrackEntry;

        public Animator Animator => null;

        public void Initialize(ModuleOwner owner)
        {
            _owner = owner as Agent;

            if (skeletonAnimation == null)
                skeletonAnimation = GetComponent<SkeletonAnimation>();

            if (skeletonRenderer == null)
                skeletonRenderer = GetComponent<SkeletonRenderer>();

            if (defaultSkeletonDataAsset == null && skeletonAnimation != null)
                defaultSkeletonDataAsset = skeletonAnimation.SkeletonDataAsset;

            SetSkeletonData(defaultSkeletonDataAsset);

            if (skeletonAnimation != null && !string.IsNullOrEmpty(defaultAnimation))
                skeletonAnimation.AnimationState.SetAnimation(0, defaultAnimation, defaultLoop);
        }

        public void PlayClip(int clipHash, float crossFadeDuration, float normalizedTime = 0, int layerIndex = 0)
        {
            SpineAnimationMap map = FindMap(clipHash);
            if (map == null || string.IsNullOrEmpty(map.animationName))
            {
                Debug.LogWarning($"Spine animation map not found. Hash: {clipHash}", this);
                return;
            }

            if (skeletonAnimation == null)
                return;

            SetSkeletonData(map.SkeletonDataAsset);

            _currentTrackEntry = skeletonAnimation.AnimationState.SetAnimation(layerIndex, map.animationName, map.loop);

            if (!map.loop && !string.IsNullOrEmpty(map.nextAnimationName))
            {
                if (map.NextSkeletonDataAsset == null || map.NextSkeletonDataAsset == skeletonAnimation.SkeletonDataAsset)
                {
                    skeletonAnimation.AnimationState.AddAnimation(layerIndex, map.nextAnimationName, map.nextLoop, map.nextDelay);
                }
                else
                {
                    _currentTrackEntry.Complete += _ =>
                    {
                        SetSkeletonData(map.NextSkeletonDataAsset);
                        skeletonAnimation.AnimationState.SetAnimation(layerIndex, map.nextAnimationName, map.nextLoop);
                    };
                }
            }
        }

        public void SetBool(AnimParamSO param, bool value)
        {
        }

        public void SetFloat(AnimParamSO param, float value)
        {
        }

        public void SetFloat(AnimParamSO param, float value, float dampTime, float deltaTime)
        {
        }

        public void SetInt(AnimParamSO param, int value)
        {
        }

        public void SetTrigger(AnimParamSO param)
        {
            if (param != null)
                PlayClip(param.ParamHash, 0f);
        }

        private SpineAnimationMap FindMap(int clipHash)
        {
            if (animationMaps == null)
                return null;

            foreach (SpineAnimationMap map in animationMaps)
            {
                if (map != null && map.ParamHash == clipHash)
                    return map;
            }

            return null;
        }

        private void SetSkeletonData(SkeletonDataAsset skeletonDataAsset)
        {
            if (skeletonAnimation == null || skeletonDataAsset == null)
                return;

            bool animationHasAsset = skeletonAnimation.SkeletonDataAsset == skeletonDataAsset;
            bool rendererHasAsset = skeletonRenderer == null || skeletonRenderer.SkeletonDataAsset == skeletonDataAsset;
            if (animationHasAsset && rendererHasAsset)
                return;

            skeletonAnimation.skeletonDataAsset = skeletonDataAsset;
            if (skeletonRenderer != null)
                skeletonRenderer.SkeletonDataAsset = skeletonDataAsset;

            if (skeletonRenderer != null)
                skeletonRenderer.Initialize(true);
            skeletonAnimation.Initialize(true);
        }
    }

    [Serializable]
    public class SpineAnimationMap
    {
        [SerializeField] private AnimParamSO animParam;
        [SerializeField] private SkeletonDataAsset skeletonDataAsset;
        public string animationName;
        public bool loop = true;

        [Header("After Non Loop")]
        [SerializeField] private SkeletonDataAsset nextSkeletonDataAsset;
        public string nextAnimationName;
        public bool nextLoop = true;
        public float nextDelay;

        public int ParamHash => animParam != null ? animParam.ParamHash : 0;
        public SkeletonDataAsset SkeletonDataAsset => skeletonDataAsset;
        public SkeletonDataAsset NextSkeletonDataAsset => nextSkeletonDataAsset;
    }
}
