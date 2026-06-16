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
        private Vector2 _originPos;
        
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _rectTransform = GetComponent<RectTransform>();
            _originPos = _rectTransform.anchoredPosition;
        }
        
        public void Show(PlayerDataSO item)
        {
            _playerData = item;
            nameText.text = item.NikkeName;
            profileImage.sprite = item.NikkeSprite;
            
            _rectTransform.anchoredPosition = _originPos + Vector2.up * slideOffset;
            _canvasGroup.alpha = 0f;

            LMotion.Create(_originPos + Vector2.up * slideOffset, _originPos, animDuration)
                .WithEase(Ease.OutCubic)
                .BindToAnchoredPosition(_rectTransform);

            LMotion.Create(0f, 1f, animDuration)
                .WithEase(Ease.OutCubic)
                .Bind(alpha => _canvasGroup.alpha = alpha);
        }

        public void Hide()
        {
            _playerData = null;
        }
    }
}