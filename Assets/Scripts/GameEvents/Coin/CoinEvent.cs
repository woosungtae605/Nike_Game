using CoreSystem.BusSystem;

namespace GameEvents.Coin
{
    public struct CoinEvent : IEvent
    {
        public readonly long Amount;

        public CoinEvent(long amount)
        {
            Amount = amount;
        }
    }
}
