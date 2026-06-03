using Agents.Players;
using CoreSystem.BusSystem;
using GameEvents.UI;
using LitMotion;
using Module;
using Reflex.Attributes;
using Systems;
using UnityEngine;
using UnityEngine.UI;

namespace Agents.Module
{
    public class GunCursorImage : MonoBehaviour
    {
        private PlayerInputSO _playerInputSO;

        [Header("Scale Motion")]
        [SerializeField] private float scalePower = 1.2f;
        [SerializeField] private float scaleDuration = 0.08f;

        private Image _cursorImage;
        private Vector3 _originScale;
        private MotionHandle _scaleHandle;

        public void Init(PlayerInputSO playerInputSO)
        {
            _playerInputSO = playerInputSO;
            if(_cursorImage == null)
                _cursorImage = GetComponent<Image>();

            _originScale = transform.localScale;
            
            SubscribeMousePos();
        }

        public void SetActiveFalse()
        {
            _scaleHandle.TryCancel();
            
            UnsubscribeMousePos();
            gameObject.SetActive(false);
        }

        public void ActiveTrue()
        {
            _cursorImage.transform.position = _playerInputSO.CurrentMousePosition;
            _playerInputSO.OnMousePos -= HandleMousePos;
            _playerInputSO.OnMousePos += HandleMousePos;
        }
        private void OnDestroy()
        {
            _scaleHandle.TryCancel();
            UnsubscribeMousePos();
        }
        
        private void SubscribeMousePos()
        {
            if (_playerInputSO == null)
                return;

            _playerInputSO.OnMousePos -= HandleMousePos;
            _playerInputSO.OnMousePos += HandleMousePos;
        }
        
        private void UnsubscribeMousePos()
        {
            if (_playerInputSO == null)
                return;

            _playerInputSO.OnMousePos -= HandleMousePos;
        }

        private void HandleMousePos(Vector2 obj)
        {
            _cursorImage.transform.position = obj;
        }

        public void PlayScaleMotion()
        {
            _scaleHandle.TryCancel();
            transform.localScale = _originScale;

            _scaleHandle = LMotion.Create(1f, scalePower, scaleDuration)
                .WithEase(Ease.OutCubic)
                .WithLoops(2, LoopType.Yoyo)
                .Bind(scale => transform.localScale = _originScale * scale);
        }
    }
}