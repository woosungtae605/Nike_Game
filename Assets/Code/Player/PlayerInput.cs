using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Player
{
    [CreateAssetMenu(fileName = "inputSystem", menuName = "SO/inputSystem", order = 0)]
    public class PlayerInput : ScriptableObject, InputSystem_Actions.IPlayerActions
    {
        public Vector3 MoveDir { get; private set; }
        
        private InputSystem_Actions _inputSo;
        private void OnEnable()
        {
            if (_inputSo == null)
            {
                _inputSo = new InputSystem_Actions();
                _inputSo.Player.SetCallbacks(this);
            }
            
            _inputSo.Player.Enable();
        }

        private void OnDisable()
        {
            _inputSo.Player.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            MoveDir = context.ReadValue<Vector3>();
        }
    }
}