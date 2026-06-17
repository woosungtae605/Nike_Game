using System;
using Agents.Players;
using LitMotion;
using LitMotion.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainSceneUI.Nikkes.NikkeProfiles
{
    public class NikkeProfile : MonoBehaviour, IUIElement<PlayerDataSO>
    {
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Image profileImage;
        [SerializeField] private Button clickBtn;
        
        [Header("Settings")]
        [SerializeField] private float animDuration = 0.5f;
        [SerializeField] private float slideOffset = 100f;
        
        private PlayerDataSO _playerData;
        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;
        
        public event Action<PlayerDataSO> OnClickBtn;
        public PlayerDataSO PlayerData => _playerData;
        
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            if (_canvasGroup == null)
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();

            _rectTransform = GetComponent<RectTransform>();
            clickBtn.onClick.AddListener(SendPlayerData);
        }

        private void OnDestroy()
        {
            clickBtn.onClick.RemoveListener(SendPlayerData);
        }

        private void SendPlayerData()
        {
            OnClickBtn?.Invoke(_playerData);
        }

        public void Show(PlayerDataSO item)
        {
            Show(item, 1);
        }

        public void Show(PlayerDataSO item, int level)
        {
            gameObject.SetActive(true);

            _playerData = item;
            nameText.text = item.NikkeName;
            profileImage.sprite = item.NikkeSprite;
            SetLevel(level);
            _canvasGroup.alpha = 0f;
        }

        public void SetLevel(int level)
        {
            if (levelText != null)
                levelText.text = level.ToString();
        }

        public void PlayShowAnimation()
        {
            Vector2 originPos = _rectTransform.anchoredPosition;
            _rectTransform.anchoredPosition = originPos + Vector2.up * slideOffset;
            _canvasGroup.alpha = 0f;

            LMotion.Create(originPos + Vector2.up * slideOffset, originPos, animDuration)
                .WithEase(Ease.OutCubic)
                .BindToAnchoredPosition(_rectTransform);

            LMotion.Create(0f, 1f, animDuration)
                .WithEase(Ease.OutCubic)
                .Bind(alpha => _canvasGroup.alpha = alpha);
        }

        public void Hide()
        {
            _playerData = null;
            if (_canvasGroup != null)
                _canvasGroup.alpha = 0f;

            gameObject.SetActive(false);
        }
    }
}
