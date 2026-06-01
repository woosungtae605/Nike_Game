using CoreSystem.BusSystem;

namespace GameEvents.UI
{
    public struct NikkeReloadUIActiveEvent : IEvent
    {
        public readonly float ReloadTime;
        public readonly bool IsActive;
        public NikkeReloadUIActiveEvent(float reloadTime, bool isActive) // if isActive = false, ignore reloadTime
        {
            this.ReloadTime = reloadTime;
            this.IsActive = isActive;
        }
    }
}