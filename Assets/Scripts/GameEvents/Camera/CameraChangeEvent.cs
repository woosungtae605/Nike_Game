using CoreSystem.BusSystem;
using UnityEngine;

namespace GameEvents.Camera
{
    public struct CameraChangeEvent : IEvent
    {
        public readonly Transform TargetTransform;
        public readonly float Duration;
        public CameraChangeEvent(Transform targetTransform, float duration)
        {
            TargetTransform = targetTransform;
            Duration = duration;
        }
    }
}