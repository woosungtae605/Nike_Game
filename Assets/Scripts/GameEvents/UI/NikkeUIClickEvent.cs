using CoreSystem.BusSystem;
using UnityEngine;

namespace GameEvents.UI
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