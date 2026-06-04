using System;
using CoreSystem.BusSystem;
using GameEvents.UI;
using Reflex.Attributes;
using Systems;
using UnityEngine;

namespace UI.BattleUI.NikkeShotUI
{
    public class HitCursorUICanvas : MonoBehaviour
    {
        [Inject] private PlayerInputSO _playerInputSO;
        private HitCursorUI _hitCursorUI;
        private void Awake()
        {
            _hitCursorUI = GetComponentInChildren<HitCursorUI>(true);
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
            _hitCursorUI.gameObject.transform.position = obj;
        }
        
        private void HandleHitCursorUI(HitCursorUIEvent obj)
        {
            _hitCursorUI.gameObject.SetActive(true);
            _hitCursorUI.UIMotion(obj.IsCritical);
        }
    }
}