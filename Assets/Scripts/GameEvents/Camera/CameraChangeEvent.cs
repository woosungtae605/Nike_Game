using CoreSystem.BusSystem;
using UnityEngine;

namespace GameEvents.Camera
{
    public struct CameraChangeEvent : IEvent
    {
        public readonly Transform TargetTransform;
        public CameraChangeEvent(Transform targetTransform)
        {
            TargetTransform = targetTransform;
        }
    }
}