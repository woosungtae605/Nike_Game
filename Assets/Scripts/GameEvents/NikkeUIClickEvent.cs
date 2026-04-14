using CoreSystem;
using UnityEngine;

namespace GameEvents
{
    public struct NikkeUIClickEvent : IEvent
    {
        public readonly Transform TargetTransform;
        public NikkeUIClickEvent(Transform targetTransform)
        {
            TargetTransform = targetTransform;
        }
    }
}