using CoreSystem.BusSystem;

namespace GameEvents.UI
{
    public struct GunAmmoUIActiveEvent : IEvent
    {
        public readonly int CurrentAmmo;
        public readonly int MaxAmmo;
        public readonly bool Active;

        public GunAmmoUIActiveEvent(int currentAmmo, int maxAmmo, bool active)
        {
            CurrentAmmo = currentAmmo;
            MaxAmmo = maxAmmo;
            Active = active;
        }
    }
}