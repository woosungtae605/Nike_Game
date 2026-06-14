using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI.BattleUI.StartUIs
{
    public class StartUICanvas : MonoBehaviour
    {
        [SerializeField] private RectTransform image1;
        [SerializeField] private RectTransform image2;
        [SerializeField] private TextMeshProUGUI bigText;
        [SerializeField] private TextMeshProUGUI smallText;
        
        [SerializeField] private float scaleDuration = 0.35f;
        [SerializeField] private float image1TargetScale = 0.85f;
        [SerializeField] private float image2TargetScale = 0.65f;
        [SerializeField] private float blinkScaleDownAmount = 0.05f;
        
        [SerializeField] private float textMoveDistance = 40f;
        [SerializeField] private float textMoveDuration = 0.35f;
        
        [SerializeField] private float blinkDuration = 0.08f;
        [SerializeField] private int blinkCount = 4;
        [SerializeField] private float blinkMinAlpha = 0.25f;

        private CanvasGroup _image1CanvasGroup;
        private CanvasGroup _image2CanvasGroup;
        private CanvasGroup _smallTextCanvasGroup;
        
        private Vector2 _smallTextOriginPos;
        
        private Sequence _scaleSequence;

        private void Awake()
        {
            _smallTextOriginPos = smallText.rectTransform.anchoredPosition;
            
            _image1CanvasGroup = GetOrAddCanvasGroup(image1.gameObject);
            _image2CanvasGroup = GetOrAddCanvasGroup(image2.gameObject);
            _smallTextCanvasGroup = GetOrAddCanvasGroup(smallText.gameObject);
        }
        
        private CanvasGroup GetOrAddCanvasGroup(GameObject target)
        {
            if (target.TryGetComponent(out CanvasGroup canvasGroup))
                return canvasGroup;

            return target.AddComponent<CanvasGroup>();
        }
        
        private void Start()
        {
            StartCoroutine(GoAction());
        }

        private IEnumerator GoAction()
        {
            PlayScaleDown();

            yield return _scaleSequence.WaitForCompletion();

            Tween smallTextTween = PlaySmallTextUp();

            yield return smallTextTween.WaitForCompletion();
        }

        public void PlayScaleDown()
        {
            _scaleSequence?.Kill();

            _scaleSequence = DOTween.Sequence();

            _scaleSequence.Join(
                image1.DOScale(image1TargetScale, scaleDuration)
                    .SetEase(Ease.OutCubic)
            );

            _scaleSequence.Join(
                image2.DOScale(image2TargetScale, scaleDuration)
                    .SetEase(Ease.OutCubic)
            );
        }
        
        private Tween PlaySmallTextUp()
        {
            RectTransform smallTextRect = smallText.rectTransform;

            smallTextRect.anchoredPosition = _smallTextOriginPos + Vector2.down * textMoveDistance;

            Sequence sequence = DOTween.Sequence();

            sequence.Join(
                smallTextRect.DOAnchorPos(_smallTextOriginPos, textMoveDuration)
                    .SetEase(Ease.OutCubic)
            );
            smallText.text = "LOADING END";
            sequence.Join(CreateBlinkTween(_image1CanvasGroup));
            sequence.Join(CreateBlinkTween(_image2CanvasGroup));
            sequence.Join(CreateBlinkTween(_smallTextCanvasGroup));
            
            sequence.Join(CreateBlinkScaleTween(image1));
            sequence.Join(CreateBlinkScaleTween(image2));

            return sequence;
        }
        
        
        private Tween CreateBlinkTween(CanvasGroup canvasGroup)
        {
            canvasGroup.alpha = 1f;

            return canvasGroup.DOFade(blinkMinAlpha, blinkDuration)
                .SetLoops(blinkCount * 2, LoopType.Yoyo)
                .OnComplete(() => canvasGroup.alpha = 1f);
        }
        
        private Tween CreateBlinkScaleTween(RectTransform rect)
        {
            Vector3 targetScale = rect.localScale - Vector3.one * blinkScaleDownAmount;

            return rect.DOScale(targetScale, blinkDuration * blinkCount)
                .SetEase(Ease.OutCubic);
        }
    }
}