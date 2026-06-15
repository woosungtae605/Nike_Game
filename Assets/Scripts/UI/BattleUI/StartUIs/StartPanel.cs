using DG.Tweening;
using UnityEngine;

namespace UI.BattleUI.StartUIs
{
    public class StartPanel : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private float fadeDuration = 0.6f;
        [SerializeField, Range(0f, 1f)] private float targetAlpha = 0.25f;

        private CanvasGroup _panelCanvasGroup;
        private Tween _fadeTween;

        private void Awake()
        {
            if (panel == null)
                return;

            _panelCanvasGroup = GetOrAddCanvasGroup(panel);
        }

        private void Start()
        {
            PlayFadeDown();
        }

        private void OnDisable()
        {
            _fadeTween?.Kill();
        }

        public void PlayFadeDown()
        {
            if (_panelCanvasGroup == null)
                return;

            _fadeTween?.Kill();
            _panelCanvasGroup.alpha = 1f;

            _fadeTween = _panelCanvasGroup.DOFade(targetAlpha, fadeDuration)
                .SetEase(Ease.Linear);
        }

        private CanvasGroup GetOrAddCanvasGroup(GameObject target)
        {
            if (target.TryGetComponent(out CanvasGroup canvasGroup))
                return canvasGroup;

            return target.AddComponent<CanvasGroup>();
        }
    }
}
