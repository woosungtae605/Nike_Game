using System;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

namespace UI.BattleUI.WaveUI.WarningUI
{
    public class WarningCanvas : MonoBehaviour
    {
        [SerializeField] private GameObject warning;
        [SerializeField] private float moveDistance = 90f;
        [SerializeField] private float showDuration = 0.28f;
        [SerializeField] private float hideDuration = 0.22f;

        private readonly CompositeMotionHandle _motions = new();
        private RectTransform _warningRect;
        private Vector2 _shownPosition;
        private Vector3 _shownScale;

        public event Action OnMotionEnd;

        public void Init()
        {
            if (warning == null)
                return;

            _warningRect = warning.transform as RectTransform;
            if (_warningRect == null)
                return;

            _shownPosition = _warningRect.anchoredPosition;
            _shownScale = _warningRect.localScale;
            HideImmediate();
        }

        private void OnDisable()
        {
            _motions.Cancel();
        }

        [ContextMenu("Show")]
        public void Show()
        {
            if (_warningRect == null)
                return;

            _motions.Cancel();
            warning.SetActive(true);

            _warningRect.anchoredPosition = _shownPosition + Vector2.up * moveDistance;
            _warningRect.localScale = new Vector3(_shownScale.x, 0f, _shownScale.z);

            LMotion.Create(_warningRect.anchoredPosition, _shownPosition, showDuration)
                .WithEase(Ease.OutCubic)
                .BindToAnchoredPosition(_warningRect)
                .AddTo(_motions);

            LMotion.Create(_warningRect.localScale, _shownScale, showDuration)
                .WithEase(Ease.OutBack)
                .BindToLocalScale(_warningRect)
                .AddTo(_motions);
            
            _motions.Cancel();

            Vector2 hidePosition = _shownPosition + Vector2.up * moveDistance;
            Vector3 hideScale = new Vector3(_shownScale.x, 0f, _shownScale.z);

            LMotion.Create(_warningRect.anchoredPosition, hidePosition, hideDuration)
                .WithEase(Ease.InCubic)
                .BindToAnchoredPosition(_warningRect)
                .AddTo(_motions);

            LMotion.Create(_warningRect.localScale, hideScale, hideDuration)
                .WithEase(Ease.InCubic)
                .BindToLocalScale(_warningRect)
                .AddTo(_motions);
            
            OnMotionEnd?.Invoke();
        }

        private void HideImmediate()
        {
            warning.SetActive(false);
            _warningRect.anchoredPosition = _shownPosition + Vector2.up * moveDistance;
            _warningRect.localScale = new Vector3(_shownScale.x, 0f, _shownScale.z);
        }
    }
}

