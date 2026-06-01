using System;
using CoreSystem.BusSystem;
using GameEvents.UI;
using Reflex.Attributes;
using Systems;
using UnityEngine;
using UnityEngine.UI;

namespace UI.BattleUI.NikkeShotUI
{
    public class GunCursorImage : MonoBehaviour
    {
        [Inject] private PlayerInputSO _playerInputSO;
        
        private Image _cursorImage;

        private void Awake()
        {
            if(_cursorImage == null)
                _cursorImage = GetComponent<Image>();

            _playerInputSO.OnMousePos += HandleMousePos;
            Bus<CursorImageChangeEvent>.OnEvent += HandleCursorImageChange;
        }

        private void HandleCursorImageChange(CursorImageChangeEvent obj)
        {
            ChangeCursorImage(obj.CursorSprite);
        }

        private void OnDestroy()
        {
            _playerInputSO.OnMousePos -= HandleMousePos;
            Bus<CursorImageChangeEvent>.OnEvent -= HandleCursorImageChange;
        }

        private void HandleMousePos(Vector2 obj)
        {
            _cursorImage.transform.position = obj;
        }

        public void ChangeCursorImage(Sprite cursorSprite)
        {
            _cursorImage.sprite = cursorSprite;
        }
    }
}