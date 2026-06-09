using Unity.Behavior;

namespace Agents.Enemies
{
    [BlackboardEnum]
    public enum EnemyState
    {
        IDLE, MOVE, ATTACK, DEATH, JUMP
    }
}