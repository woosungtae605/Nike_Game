using Agents.Enemies;
using Module;
using Systems;
using Systems.AnimationSystems;
using UnityEngine;

namespace Agents.Module
{
    public class PlayerMoveInput : MonoBehaviour, IModule, IAfterInitModule
    {
        [SerializeField] private PlayerInputSO inputSO;
        [SerializeField] private AnimParamSO _walkHash;
        [SerializeField] private AnimParamSO _idleHash;
        
        private INavMovement _navMovement;
        private RendererModule agentRenderer;
        private ModuleOwner Owner;

        private bool isMoving;

        public void Initialize(ModuleOwner owner)
        {
            Owner = owner;
        }

        public void AfterInit()
        {
            _navMovement = Owner.GetModule<INavMovement>();
            agentRenderer = Owner.GetModule<RendererModule>();
        }

        private void OnEnable()
        {
            if (inputSO != null)
                inputSO.OnLeftMousePressedStart += HandleLeftMouseClick;
        }

        private void OnDisable()
        {
            if (inputSO != null)
                inputSO.OnLeftMousePressedStart -= HandleLeftMouseClick;
        }

        private void Update()
        {
            if (_navMovement == null || agentRenderer == null) return;

            if (isMoving && _navMovement.IsArrived)
            {
                isMoving = false;
                agentRenderer.PlayClip(_idleHash.ParamHash, 0, 0);
            }
        }

        private void HandleLeftMouseClick()
        {
            Vector3 mouseWorldPosition = inputSO.GetWorldMousePosition();

            _navMovement.SetDestination(mouseWorldPosition);

            if (!isMoving)
            {
                isMoving = true;
                agentRenderer.PlayClip(_walkHash.ParamHash, 0, 0);
            }
        }
    }
}