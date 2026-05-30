using FSM;
using Systems.AnimationSystems;
using UnityEngine;

namespace Agents.Players.States
{
    public class AbstractPlayerState : AgentState
    {
        protected Player Player;
        public AbstractPlayerState(Agent owner, AnimParamSO stateParam) : base(owner, stateParam)
        {
            Player = owner as Player;
            if (Player == null)
            {
                Debug.LogError("AbstractPlayerState requires Player owner.");
            }
        }
    }
}