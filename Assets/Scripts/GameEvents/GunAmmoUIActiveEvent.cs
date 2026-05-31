using CoreSystem;

namespace GameEvents
{
    public struct GunAmmoUIActiveEvent : IEvent
    {
        public readonly int CurrentAmmo;
        public readonly int MaxAmmo;

        public GunAmmoUIActiveEvent(int currentAmmo, int maxAmmo)
        {
            CurrentAmmo = currentAmmo;
            MaxAmmo = maxAmmo;
        }
    }
}