using CoreSystem;

namespace GameEvents
{
    public struct CameraRecoilEvent : IEvent
    {
        public readonly float Power;
        public readonly float Duration;
        public readonly bool RotationRecoil;
        public readonly bool PositionRecoil;
        
        public CameraRecoilEvent(float power, float duration, bool rotationRecoil, bool positionRecoil)
        {
            Power = power;
            Duration = duration;
            RotationRecoil = rotationRecoil;
            PositionRecoil = positionRecoil;
        }
    }
}