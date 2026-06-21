using Agents.Enemies;
using CoreSystem.BusSystem;

namespace GameEvents.UI
{
    public struct ClearUIEvent : IEvent
    {
    }

    public struct FailUIEvent : IEvent
    {
    }

    public struct TutorialEnemySpawnEvent : IEvent
    {
        public AbstractEnemy Enemy { get; }

        public TutorialEnemySpawnEvent(AbstractEnemy enemy)
        {
            Enemy = enemy;
        }
    }

    public struct TutorialBossSpawnEvent : IEvent
    {
        public AbstractEnemy Boss { get; }

        public TutorialBossSpawnEvent(AbstractEnemy boss)
        {
            Boss = boss;
        }
    }
}
