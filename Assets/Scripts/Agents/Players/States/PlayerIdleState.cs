using FSM;
using Systems.AnimationSystems;

namespace Agents.Players.States
{
    public class PlayerIdleState : AgentState
    {
        public PlayerIdleState(Agent owner, AnimParamSO stateParam) : base(owner, stateParam)
        {
        }
    }
}