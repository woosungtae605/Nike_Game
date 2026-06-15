using System;
using CoreSystem.BusSystem;
using GameEvents.UI;
using LitMotion;
using LitMotion.Extensions;
using Systems.GameSystem.Wave;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.BattleUI
{
    public class ClearUICanvas : MonoBehaviour, IUIElement
    {
        [SerializeField] private WaveManager waveManager;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private RectTransform panelRoot;
        [SerializeField] private RectTransform titleRoot;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text subText;
        [SerializeField] private Button confirmButton;
        [SerializeField] private float fadeDuration = 0.22f;
        [SerializeField] private float panelMoveDuration = 0.35f;
        [SerializeField] private float titlePunchDuration = 0.28f;

        private readonly CompositeMotionHandle _motions = new();
        private Vector2 _panelShownPosition;
        private Vector2 _titleShownPosition;
        private float _titleFontSize;

        private void Awake()
        {
            if (canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();

            if (confirmButton == null)
                confirmButton = GetComponentInChildren<Button>(true);

            CacheDefaults();
            HideImmediate();

            Bus<ClearUIEvent>.OnEvent += HandleClearUI;
        }

        private void OnDestroy()
        {
            Bus<ClearUIEvent>.OnEvent -= HandleClearUI;
        }

        private void HandleClearUI(ClearUIEvent obj)
        {
            Show();
        }

        private void OnEnable()
        {
            if (waveManager != null)
                waveManager.OnClear += Show;

            if (confirmButton != null)
            {
                confirmButton.onClick.AddListener(Hide);
                confirmButton.onClick.AddListener(ClearNext);
            }
        }

        private void ClearNext()
        {
            
        }

        private void OnDisable()
        {
            if (waveManager != null)
                waveManager.OnClear -= Show;

            if (confirmButton != null)
            {
                confirmButton.onClick.RemoveListener(Hide);
                confirmButton.onClick.RemoveListener(ClearNext);   
            }

            _motions.Cancel();
        }

        [ContextMenu("Show")]
        public void Show()
        {
            CacheDefaults();
            _motions.Cancel();

            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
                LMotion.Create(0f, 1f, fadeDuration)
                    .WithEase(Ease.OutCubic)
                    .BindToAlpha(canvasGroup)
                    .AddTo(_motions);
            }

            if (panelRoot != null)
            {
                panelRoot.anchoredPosition = _panelShownPosition + new Vector2(-180f, 0f);
                LMotion.Create(panelRoot.anchoredPosition, _panelShownPosition, panelMoveDuration)
                    .WithEase(Ease.OutExpo)
                    .BindToAnchoredPosition(panelRoot)
                    .AddTo(_motions);
            }

            if (titleRoot != null)
            {
                titleRoot.anchoredPosition = _titleShownPosition + new Vector2(70f, 0f);
                LMotion.Create(titleRoot.anchoredPosition, _titleShownPosition, titlePunchDuration)
                    .WithEase(Ease.OutBack)
                    .BindToAnchoredPosition(titleRoot)
                    .AddTo(_motions);
            }

            if (titleText != null)
            {
                titleText.fontSize = _titleFontSize * 0.88f;
                LMotion.Create(titleText.fontSize, _titleFontSize, titlePunchDuration)
                    .WithEase(Ease.OutBack)
                    .BindToFontSize(titleText)
                    .AddTo(_motions);
            }
        }

        public void Hide()
        {
            _motions.Cancel();
            HideImmediate();
        }

        private void CacheDefaults()
        {
            if (panelRoot != null)
                _panelShownPosition = panelRoot.anchoredPosition;

            if (titleRoot != null)
                _titleShownPosition = titleRoot.anchoredPosition;

            if (titleText != null && _titleFontSize <= 0f)
                _titleFontSize = titleText.fontSize;
        }

        private void HideImmediate()
        {
            if (canvasGroup == null)
                return;

            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
}
