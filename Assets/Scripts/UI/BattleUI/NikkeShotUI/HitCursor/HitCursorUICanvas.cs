using CoreSystem.BusSystem;
using GameEvents.UI;
using Reflex.Attributes;
using Systems;
using UnityEngine;

namespace UI.BattleUI.NikkeShotUI.HitCursor
{
    public class HitCursorUICanvas : MonoBehaviour
    {
        [Inject] private PlayerInputSO _playerInputSO;
        private HitCursorUI _hitCursorUI;
        private Vector2 _currentMousePosition;

        private void Awake()
        {
            _hitCursorUI = GetComponentInChildren<HitCursorUI>(true);
            _hitCursorUI.gameObject.SetActive(false);
            Bus<HitCursorUIEvent>.OnEvent += HandleHitCursorUI;
            _playerInputSO.OnMousePos += HandleMousePos;
        }

        private void OnDestroy()
        {
            Bus<HitCursorUIEvent>.OnEvent -= HandleHitCursorUI;
            _playerInputSO.OnMousePos -= HandleMousePos;
        }
        
        private void HandleMousePos(Vector2 obj)
        {
            _currentMousePosition = obj;

            if (_hitCursorUI == null)
                return;

            _hitCursorUI.transform.position = obj;
        }
        
        private void HandleHitCursorUI(HitCursorUIEvent obj)
        {
            if (_hitCursorUI == null)
                return;

            _hitCursorUI.transform.position = _currentMousePosition;
            _hitCursorUI.UIMotion(obj.IsCritical);
        }
    }
}
