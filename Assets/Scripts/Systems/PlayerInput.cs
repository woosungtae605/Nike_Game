using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Systems
{
    [CreateAssetMenu(fileName = "inputSystem", menuName = "SO/inputSystem", order = 0)]
    public class PlayerInput : ScriptableObject, Controller.IPlayerActions
    {
        public event Action<Vector2> OnMovementPressed;
        public event Action OnLeftMousePressedStart;
        public event Action OnLeftMousePressedEnd;
        
        private Controller _inputSo;
        private void OnEnable()
        {
            if (_inputSo == null)
            {
                _inputSo = new Controller();
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
            Vector2 moveDir = context.ReadValue<Vector2>();
            OnMovementPressed?.Invoke(moveDir);
        }

        public void OnMouseClick(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnLeftMousePressedStart?.Invoke();
            if(context.canceled)
                OnLeftMousePressedEnd?.Invoke();
        }
    }
}