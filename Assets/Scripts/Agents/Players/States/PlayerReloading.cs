using FSM;
using Systems.AnimationSystems;

namespace Agents.Players.States
{
    public class PlayerReloading : AbstractPlayerState
    {
        public PlayerReloading(Agent owner, AnimParamSO stateParam) : base(owner, stateParam)
        {
        }
    }
}