using FSM;
using Systems.AnimationSystems;
using UnityEngine;

namespace Agents.Players.States
{
    public class PlayerShootingState : AbstractPlayerState
    {
        public PlayerShootingState(Agent owner, AnimParamSO stateParam) : base(owner, stateParam)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Player.PlayerInputSo.OnLeftMousePressedEnd += HandleMousePressedEnd;
        }

        public override void Update()
        {
            base.Update();
            Player.PlayerGunCompo.TryFire();
        }

        public override void Exit()
        {
            Player.PlayerInputSo.OnLeftMousePressedEnd -= HandleMousePressedEnd;
            base.Exit();
        }

        private void HandleMousePressedEnd()
        {
            Player.ChangeState(PlayerStates.IDLE);
        }
    }
}