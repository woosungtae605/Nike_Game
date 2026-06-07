using CoreSystem.BusSystem;
using GameEvents.UI;
using Reflex.Attributes;
using Systems;
using UnityEngine;

namespace UI.BattleUI.NikkeShotUI.HitCursor
{
    public class HitCursorUICanvas : MonoBehaviour
    {
        private HitCursorUI _hitCursorUI;
        private UIFollowMouse _uiFollowMouse;

        private void Awake()
        {
            _hitCursorUI = GetComponentInChildren<HitCursorUI>(true);
            _uiFollowMouse = _hitCursorUI.GetComponent<UIFollowMouse>();
            
            _hitCursorUI.gameObject.SetActive(false);
            
            Bus<HitCursorUIEvent>.OnEvent += HandleHitCursorUI;
        }

        private void OnDestroy()
        {
            Bus<HitCursorUIEvent>.OnEvent -= HandleHitCursorUI;
        }
        
        private void HandleHitCursorUI(HitCursorUIEvent obj)
        {
            if (_hitCursorUI == null)
                return;

            _uiFollowMouse?.SnapToCurrentMousePosition();
            _hitCursorUI.UIMotion(obj.IsCritical);
        }
    }
}
