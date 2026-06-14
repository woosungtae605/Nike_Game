using System.Collections;
using LitMotion;
using LitMotion.Extensions;
using TMPro;
using UnityEngine;

namespace UI.BattleUI.WaveUI.WarningUI
{
    public class WarningCanvas : MonoBehaviour
    {
        [SerializeField] private GameObject warning;
        [SerializeField] private TextMeshProUGUI[] warningText;
        [SerializeField] private float moveDistance = 90f;
        [SerializeField] private float showDuration = 0.28f;
        [SerializeField] private float holdDuration = 0.25f;
        [SerializeField] private float hideDuration = 0.22f;
        [SerializeField, Range(0.1f, 1f)] private float textMinAlphaRatio = 0.55f;
        [SerializeField] private float textBlinkDuration = 0.18f;

        private readonly CompositeMotionHandle _motions = new();
        private readonly CompositeMotionHandle _textMotions = new();
        private RectTransform _warningRect;
        private Vector2 _shownPosition;
        private Vector3 _shownScale;
        private float[] _textDefaultAlphas;
        private Coroutine _showCoroutine;

        public void Init()
        {
            if (warning == null)
                return;

            _warningRect = warning.transform as RectTransform;
            if (_warningRect == null)
                return;

            _shownPosition = _warningRect.anchoredPosition;
            _shownScale = _warningRect.localScale;
            CacheTextAlphas();
            HideImmediate();
        }

        private void OnDisable()
        {
            _motions.Cancel();
            StopTextBlink();

            if (_showCoroutine != null)
            {
                StopCoroutine(_showCoroutine);
                _showCoroutine = null;
            }
        }

        [ContextMenu("Show")]
        public void Show()
        {
            if (_showCoroutine != null)
                StopCoroutine(_showCoroutine);

            _showCoroutine = StartCoroutine(ShowRoutine());
        }

        public IEnumerator ShowRoutine()
        {
            if (_warningRect == null)
                yield break;

            _motions.Cancel();
            StopTextBlink();
            warning.SetActive(true);
            StartTextBlink();

            _warningRect.anchoredPosition = _shownPosition + Vector2.up * moveDistance;
            _warningRect.localScale = new Vector3(_shownScale.x, 0f, _shownScale.z);

            LMotion.Create(_warningRect.anchoredPosition, _shownPosition, showDuration)
                .WithEase(Ease.OutCubic)
                .BindToAnchoredPosition(_warningRect)
                .AddTo(_motions);

            MotionHandle showScaleHandle = LMotion.Create(_warningRect.localScale, _shownScale, showDuration)
                .WithEase(Ease.OutBack)
                .BindToLocalScale(_warningRect)
                .AddTo(_motions);

            yield return showScaleHandle.ToYieldInstruction();

            if (holdDuration > 0f)
                yield return new WaitForSeconds(holdDuration);

            StopTextBlink();

            Vector2 hidePosition = _shownPosition + Vector2.up * moveDistance;
            Vector3 hideScale = new Vector3(_shownScale.x, 0f, _shownScale.z);

            LMotion.Create(_warningRect.anchoredPosition, hidePosition, hideDuration)
                .WithEase(Ease.InCubic)
                .BindToAnchoredPosition(_warningRect)
                .AddTo(_motions);

            MotionHandle hideScaleHandle = LMotion.Create(_warningRect.localScale, hideScale, hideDuration)
                .WithEase(Ease.InCubic)
                .BindToLocalScale(_warningRect)
                .AddTo(_motions);

            yield return hideScaleHandle.ToYieldInstruction();

            warning.SetActive(false);
            _showCoroutine = null;
        }

        private void CacheTextAlphas()
        {
            if (warningText == null)
                return;

            _textDefaultAlphas = new float[warningText.Length];
            for (int i = 0; i < warningText.Length; i++)
            {
                if (warningText[i] == null)
                    continue;

                _textDefaultAlphas[i] = warningText[i].color.a;
            }
        }

        private void StartTextBlink()
        {
            if (warningText == null)
                return;

            if (_textDefaultAlphas == null || _textDefaultAlphas.Length != warningText.Length)
                CacheTextAlphas();

            _textMotions.Cancel();
            for (int i = 0; i < warningText.Length; i++)
            {
                TextMeshProUGUI text = warningText[i];
                if (text == null)
                    continue;

                float defaultAlpha = _textDefaultAlphas[i];
                float minAlpha = defaultAlpha * textMinAlphaRatio;
                SetTextAlpha(text, defaultAlpha);

                LMotion.Create(defaultAlpha, minAlpha, textBlinkDuration)
                    .WithEase(Ease.InOutSine)
                    .WithLoops(-1, LoopType.Yoyo)
                    .BindToColorA(text)
                    .AddTo(_textMotions);
            }
        }

        private void StopTextBlink()
        {
            _textMotions.Cancel();
            RestoreTextAlphas();
        }

        private void RestoreTextAlphas()
        {
            if (warningText == null || _textDefaultAlphas == null)
                return;

            int count = Mathf.Min(warningText.Length, _textDefaultAlphas.Length);
            for (int i = 0; i < count; i++)
            {
                if (warningText[i] == null)
                    continue;

                SetTextAlpha(warningText[i], _textDefaultAlphas[i]);
            }
        }

        private void SetTextAlpha(TextMeshProUGUI text, float alpha)
        {
            Color color = text.color;
            color.a = alpha;
            text.color = color;
        }

        private void HideImmediate()
        {
            warning.SetActive(false);
            _warningRect.anchoredPosition = _shownPosition + Vector2.up * moveDistance;
            _warningRect.localScale = new Vector3(_shownScale.x, 0f, _shownScale.z);
            RestoreTextAlphas();
        }
    }
}
