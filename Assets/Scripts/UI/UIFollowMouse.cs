using Reflex.Attributes;
using Systems;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class UIFollowMouse : MonoBehaviour
    {
        [SerializeField, Inject] private PlayerInputSO _playerInputSO;

        private void OnEnable()
        {
            if (_playerInputSO != null)
                _playerInputSO.OnMousePos += HandleMousePos;
        }

        private void OnDisable()
        {
            if (_playerInputSO != null)
                _playerInputSO.OnMousePos -= HandleMousePos;
        }

        private void Update()
        {
            if (_playerInputSO == null)
                transform.position = GetCurrentMousePosition();
        }

        private void HandleMousePos(Vector2 mousePosition)
        {
            transform.position = mousePosition;
        }

        public void SnapToCurrentMousePosition()
        {
            transform.position = GetCurrentMousePosition();
        }

        private Vector2 GetCurrentMousePosition()
        {
            if (_playerInputSO != null)
                return _playerInputSO.CurrentMousePosition;

            return Mouse.current != null ? Mouse.current.position.ReadValue() : Vector2.zero;
        }
    }
}
