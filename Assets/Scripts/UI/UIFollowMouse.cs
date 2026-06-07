using Reflex.Attributes;
using Systems;
using UnityEngine;

namespace UI
{
    public class UIFollowMouse : MonoBehaviour
    {
        [Inject] private PlayerInputSO _playerInputSO;
        

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

        private void HandleMousePos(Vector2 mousePosition)
        {
            transform.position = mousePosition;
        }

        public void SnapToCurrentMousePosition()
        {
            transform.position = _playerInputSO.CurrentMousePosition;
        }
    }
}