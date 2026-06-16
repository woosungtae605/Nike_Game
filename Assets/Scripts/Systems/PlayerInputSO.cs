using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Systems
{
    [CreateAssetMenu(fileName = "inputSystem", menuName = "SO/inputSystem", order = 0)]
    public class PlayerInputSO : ScriptableObject, Controller.IPlayerActions
    {
        [SerializeField] private LayerMask whatIsGround;
        
        public event Action<Vector2> OnMovementPressed;
        public event Action OnLeftMousePressedStart;
        public event Action OnLeftMousePressedEnd;
        public event Action OnRightMousePressedStart;
        public event Action OnRightMousePressedEnd;
        public event Action<Vector2> OnMouseDeltaPos;
        public event Action<Vector2> OnMousePos;
        
        private Camera _mainCam;
        public Camera MainCam
        {
            get
            {
                if(_mainCam == null)
                    _mainCam = Camera.main;
                return _mainCam;
            }
        }
        
        public Vector2 CurrentMousePosition { get; private set; }
        
        private Vector3 _worldMousePosition;

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
        
        public Vector3 GetWorldMousePosition()
        {
            if (MainCam == null)
                return _worldMousePosition;
            Ray camRay = MainCam.ScreenPointToRay(CurrentMousePosition);
            if (Physics.Raycast(camRay, out RaycastHit hit, MainCam.farClipPlane, whatIsGround))
            {
                _worldMousePosition = hit.point;
            }
            return _worldMousePosition;
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

        public void OnMouseDeltaPosition(InputAction.CallbackContext context)
        {
            Vector2 delta = context.ReadValue<Vector2>();
            OnMouseDeltaPos?.Invoke(delta);
        }

        public void OnMousePosition(InputAction.CallbackContext context)
        {
            Vector2 mousePosition = context.ReadValue<Vector2>();
            CurrentMousePosition = mousePosition;
            OnMousePos?.Invoke(mousePosition);
        }

        public void OnRightMouseClick(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnRightMousePressedStart?.Invoke();
            if (context.canceled)
                OnRightMousePressedEnd?.Invoke();
        }
    }
}