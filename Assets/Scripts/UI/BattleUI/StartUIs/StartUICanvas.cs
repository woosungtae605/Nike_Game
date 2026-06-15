using System;
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
        [SerializeField] private TextMeshProUGUI middleText;
        
        [SerializeField] private float scaleDuration = 0.35f;
        [SerializeField] private float image1TargetScale = 0.85f;
        [SerializeField] private float image2TargetScale = 0.65f;
        [SerializeField] private float blinkScaleDownAmount = 0.05f;
        
        [SerializeField] private float textMoveDistance = 40f;
        [SerializeField] private float textMoveDuration = 0.35f;
        [SerializeField] private float middleTextStartScale = 1.35f;
        [SerializeField] private float middleTextScaleDuration = 0.35f;
        [SerializeField] private float finalScaleMultiplier = 1.45f;
        [SerializeField] private float finalFadeDuration = 0.45f;
        
        [SerializeField] private float blinkDuration = 0.08f;
        [SerializeField] private int blinkCount = 4;
        [SerializeField] private int textBlinkCount = 2;
        [SerializeField] private float blinkMinAlpha = 0.25f;

        private CanvasGroup[] _imageCanvasGroups;
        private CanvasGroup[] _textCanvasGroups;
        private CanvasGroup[] _smallTextCanvasGroups;
        private CanvasGroup[] _finalCanvasGroups;
        private CanvasGroup _middleTextCanvasGroup;
        private RectTransform[] _scaleTargets;
        private RectTransform[] _finalScaleTargets;
        
        private Vector2 _smallTextOriginPos;
        private Vector3 _middleTextOriginScale;
        
        private Sequence _scaleSequence;

        public event Action OnEnd;

        private void Awake()
        {
            _smallTextOriginPos = smallText.rectTransform.anchoredPosition;

            _imageCanvasGroups = new[]
            {
                GetOrAddCanvasGroup(image1.gameObject),
                GetOrAddCanvasGroup(image2.gameObject)
            };

            _textCanvasGroups = new[]
            {
                GetOrAddCanvasGroup(bigText.gameObject),
                GetOrAddCanvasGroup(smallText.gameObject)
            };

            _smallTextCanvasGroups = new[]
            {
                GetOrAddCanvasGroup(smallText.gameObject)
            };
            
            _middleTextCanvasGroup = GetOrAddCanvasGroup(middleText.gameObject);
            _middleTextOriginScale = middleText.rectTransform.localScale;
            middleText.gameObject.SetActive(false);
            _middleTextCanvasGroup.alpha = 0f;

            _scaleTargets = new[]
            {
                image1,
                image2
            };

            _finalCanvasGroups = new[]
            {
                _imageCanvasGroups[0],
                _imageCanvasGroups[1],
                _middleTextCanvasGroup
            };

            _finalScaleTargets = new[]
            {
                image1,
                image2,
                middleText.rectTransform
            };
        }
        
        private CanvasGroup GetOrAddCanvasGroup(GameObject target)
        {
            if (target.TryGetComponent(out CanvasGroup canvasGroup))
                return canvasGroup;

            return target.AddComponent<CanvasGroup>();
        }
        public void ActionStart()
        {
            StartCoroutine(GoAction());   
        }

        private IEnumerator GoAction()
        {
            PlayScaleDown();

            yield return _scaleSequence.WaitForCompletion();

            Tween smallTextTween = PlaySmallTextUp();

            yield return smallTextTween.WaitForCompletion();

            Tween textBlinkTween = PlayTextBlink();

            yield return textBlinkTween.WaitForCompletion();

            HideStartTexts();

            Tween middleTextTween = PlayMiddleTextShow();

            yield return middleTextTween.WaitForCompletion();

            Tween finalTween = PlayFinalOut();

            yield return finalTween.WaitForCompletion();
            
            OnEnd?.Invoke();
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
            smallText.text = "LOADING END";

            Tween moveTween = smallTextRect.DOAnchorPos(_smallTextOriginPos, textMoveDuration)
                .SetEase(Ease.OutCubic);

            Sequence effectSequence = DOTween.Sequence();
            JoinBlinkTweens(effectSequence, _imageCanvasGroups, blinkCount);
            JoinBlinkTweens(effectSequence, _smallTextCanvasGroups, blinkCount);
            JoinScaleTweens(effectSequence, _scaleTargets);

            return moveTween;
        }

        private Tween PlayTextBlink()
        {
            Sequence sequence = DOTween.Sequence();
            JoinBlinkTweens(sequence, _textCanvasGroups, textBlinkCount);
            return sequence;
        }

        private void HideStartTexts()
        {
            bigText.gameObject.SetActive(false);
            smallText.gameObject.SetActive(false);
        }

        private Tween PlayMiddleTextShow()
        {
            RectTransform middleTextRect = middleText.rectTransform;
            Sequence sequence = DOTween.Sequence();

            middleText.gameObject.SetActive(true);
            _middleTextCanvasGroup.alpha = 0f;
            middleTextRect.localScale = _middleTextOriginScale * middleTextStartScale;

            sequence.Join(
                middleTextRect.DOScale(_middleTextOriginScale, middleTextScaleDuration)
                    .SetEase(Ease.OutCubic)
            );

            sequence.Join(
                _middleTextCanvasGroup.DOFade(1f, middleTextScaleDuration * 0.5f)
                    .SetEase(Ease.OutCubic)
            );

            return sequence;
        }

        private Tween PlayFinalOut()
        {
            Sequence sequence = DOTween.Sequence();

            JoinBlinkTweens(sequence, _finalCanvasGroups, textBlinkCount);
            sequence.Append(CreateFinalFadeOutTween());

            return sequence;
        }

        private Tween CreateFinalFadeOutTween()
        {
            Sequence sequence = DOTween.Sequence();

            foreach (RectTransform rect in _finalScaleTargets)
            {
                sequence.Join(
                    rect.DOScale(rect.localScale * finalScaleMultiplier, finalFadeDuration)
                        .SetEase(Ease.OutCubic)
                );
            }

            foreach (CanvasGroup canvasGroup in _finalCanvasGroups)
            {
                canvasGroup.alpha = 1f;
                sequence.Join(
                    canvasGroup.DOFade(0f, finalFadeDuration)
                        .SetEase(Ease.OutCubic)
                );
            }

            sequence.OnComplete(() =>
            {
                image1.gameObject.SetActive(false);
                image2.gameObject.SetActive(false);
                middleText.gameObject.SetActive(false);
            });

            return sequence;
        }

        private void JoinBlinkTweens(Sequence sequence, CanvasGroup[] canvasGroups, int count)
        {
            foreach (CanvasGroup canvasGroup in canvasGroups)
            {
                sequence.Join(CreateBlinkTween(canvasGroup, count));
            }
        }

        private void JoinScaleTweens(Sequence sequence, RectTransform[] rects)
        {
            foreach (RectTransform rect in rects)
            {
                sequence.Join(CreateBlinkScaleTween(rect));
            }
        }
        
        private Tween CreateBlinkTween(CanvasGroup canvasGroup, int count)
        {
            canvasGroup.alpha = 1f;

            return canvasGroup.DOFade(blinkMinAlpha, blinkDuration)
                .SetLoops(count * 2, LoopType.Yoyo)
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
