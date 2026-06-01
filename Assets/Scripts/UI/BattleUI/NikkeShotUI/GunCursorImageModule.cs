using System;
using Agents.Players;
using CoreSystem.BusSystem;
using GameEvents.UI;
using LitMotion;
using Module;
using Reflex.Attributes;
using Systems;
using UnityEngine;
using UnityEngine.UI;

namespace UI.BattleUI.NikkeShotUI
{
    public class GunCursorImageModule : MonoBehaviour, IModule
    {
        [Inject] private PlayerInputSO _playerInputSO;

        [Header("Scale Motion")]
        [SerializeField] private float scalePower = 1.2f;
        [SerializeField] private float scaleDuration = 0.08f;

        private Image _cursorImage;
        private Vector3 _originScale;
        private MotionHandle _scaleHandle;

        private Player _player;

        private void Awake()
        {
            if(_cursorImage == null)
                _cursorImage = GetComponent<Image>();

            _originScale = transform.localScale;

            _playerInputSO.OnMousePos += HandleMousePos;
            Bus<CursorImageChangeEvent>.OnEvent += HandleCursorImageChange;
        }

        private void HandleCursorImageChange(CursorImageChangeEvent obj)
        {
            ChangeCursorImage(obj.CursorSprite);
        }

        private void OnDestroy()
        {
            _scaleHandle.TryCancel();

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

        public void PlayScaleMotion()
        {
            _scaleHandle.TryCancel();
            transform.localScale = _originScale;

            _scaleHandle = LMotion.Create(1f, scalePower, scaleDuration)
                .WithEase(Ease.OutCubic)
                .WithLoops(2, LoopType.Yoyo)
                .Bind(scale => transform.localScale = _originScale * scale);
        }

        public void Initialize(ModuleOwner owner)
        {
            _player = owner as Player;
        }
    }
}