using Agents.Players;
using Module;
using UnityEngine;

namespace Agents.Module
{
    public class PlayerAimModule : MonoBehaviour, IModule
    {
        [SerializeField] private UnityEngine.Camera mainCamera;

        private Player _player;
        private Vector2 _mousePosition;

        public void Initialize(ModuleOwner owner)
        {
            _player = owner as Player;
            Debug.Assert(_player != null, "owner is not Player");

            if (mainCamera == null)
                mainCamera = UnityEngine.Camera.main;

            _player.PlayerInputSo.OnMousePos += HandleMousePos;
        }

        private void OnDestroy()
        {
            if (_player != null && _player.PlayerInputSo != null)
                _player.PlayerInputSo.OnMousePos -= HandleMousePos;
        }

        private void HandleMousePos(Vector2 mousePosition)
        {
            _mousePosition = mousePosition;
        }

        public Ray GetAimRay()
        {
            return mainCamera.ScreenPointToRay(_mousePosition);
        }
    }
}