using CoreSystem.BusSystem;

namespace GameEvents.UI
{
    public struct SniperChargeUIEvent : IEvent
    {
        public float Percent { get; }
        public float DisplayPercent { get; }
        public bool Active { get; }

        public SniperChargeUIEvent(float percent, float displayPercent, bool active)
        {
            Percent = percent;
            DisplayPercent = displayPercent;
            Active = active;
        }
    }
}
