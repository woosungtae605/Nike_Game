using CoreSystem;

namespace GameEvents
{
    public struct CameraRecoilEvent : IEvent
    {
        public readonly float Power;
        public readonly float Duration;
        
        public CameraRecoilEvent(float power, float duration)
        {
            Power = power;
            Duration = duration;
        }
    }
}