using Agents.Players;
using CoreSystem.BusSystem;

namespace GameEvents
{
    public struct BattleEndEvent : IEvent
    {
    }

    public struct BattleFailEvent : IEvent
    {
    }

    public struct PlayerDeathEvent : IEvent
    {
        public Player Player { get; }

        public PlayerDeathEvent(Player player)
        {
            Player = player;
        }
    }
}