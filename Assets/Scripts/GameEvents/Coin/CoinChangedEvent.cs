using CoreSystem.BusSystem;

namespace GameEvents.Coin
{
    public struct CoinChangedEvent : IEvent
    {
        public readonly long CurrentCoin;

        public CoinChangedEvent(long currentCoin)
        {
            CurrentCoin = currentCoin;
        }
    }
}
