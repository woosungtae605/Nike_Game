using CoreSystem.BusSystem;
using UnityEngine;

namespace GameEvents.Camera
{
    public struct CameraChangeEvent : IEvent
    {
        public readonly Transform TargetTransform;
        public readonly float Duration;
        public readonly int Priority;
        public readonly float LockDuration;

        public CameraChangeEvent(Transform targetTransform, float duration, int priority = 0, float lockDuration = 0f)
        {
            TargetTransform = targetTransform;
            Duration = duration;
            Priority = priority;
            LockDuration = lockDuration;
        }
    }
}
