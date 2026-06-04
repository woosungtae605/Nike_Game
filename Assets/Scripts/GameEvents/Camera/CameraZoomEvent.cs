using CoreSystem.BusSystem;
using UnityEngine;

namespace GameEvents.Camera
{
    public struct CameraZoomEvent : IEvent
    {
        public readonly float ZoomAmount;
        public readonly bool ZoomIn;
        
        public CameraZoomEvent(float zoomAmount, bool zoomIn)
        {
            ZoomAmount = zoomAmount;
            ZoomIn = zoomIn;
        }
    }
}