using FSM;
using Systems.AnimationSystems;

namespace Agents.Players.States
{
    public class PlayerShootingState : AgentState
    {
        public PlayerShootingState(Agent owner, AnimParamSO stateParam) : base(owner, stateParam)
        {
        }
    }
}