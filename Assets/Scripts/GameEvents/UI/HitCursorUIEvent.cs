using CoreSystem.BusSystem;

namespace GameEvents.UI
{
    public class HitCursorUIEvent : IEvent
    {
        public readonly bool IsCritical;
        
        public HitCursorUIEvent(bool isCritical)
        {
            IsCritical = isCritical;
        }
    }
}