using CoreSystem.BusSystem;
using GameEvents.UI;
using LitMotion;
using UnityEngine;
using UnityEngine.UI;

namespace UI.BattleUI.NikkeShotUI
{
    public class HitCursorUI : MonoBehaviour
    {
        [SerializeField] private Image[] hitCursorImages;
        [SerializeField] private float scalePower = 3f;
        [SerializeField] private float waitDuration = 0.7f;
        [SerializeField] private float motionDuration = 0.2f;
        [SerializeField] private Ease motionEase = Ease.InCubic;
        
        private RectTransform _myRectTransform;
        private Vector3 _originScale;
        private MotionHandle _motionHandle;

        private void Awake()
        {
            _myRectTransform = GetComponent<RectTransform>();
            _originScale = _myRectTransform.localScale;
            SetAlpha(0f);
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _motionHandle.TryCancel();
        }

        public void UIMotion(bool isCritical)
        {
            _motionHandle.TryCancel();

            gameObject.SetActive(true);
            _myRectTransform.localScale = _originScale;
            SetColor(isCritical ? Color.red : Color.white);
            SetAlpha(1f);

            _motionHandle = LMotion.Create(0f, 1f, motionDuration)
                .WithDelay(waitDuration)
                .WithEase(motionEase)
                .WithOnComplete(() =>
                {
                    SetAlpha(0f);
                    gameObject.SetActive(false);
                })
                .Bind(t =>
                {
                    _myRectTransform.localScale = Vector3.Lerp(_originScale, _originScale * scalePower, t);
                    SetAlpha(1f - t);
                });
        }

        private void SetAlpha(float alpha)
        {
            if (hitCursorImages == null)
                return;

            foreach (Image image in hitCursorImages)
            {
                if (image == null)
                    continue;

                Color color = image.color;
                color.a = alpha;
                image.color = color;
            }
        }
    

        private void SetColor(Color color)
        {
            if (hitCursorImages == null)
                return;

            foreach (Image image in hitCursorImages)
            {
                if (image == null)
                    continue;

                Color currentColor = image.color;
                currentColor.r = color.r;
                currentColor.g = color.g;
                currentColor.b = color.b;
                image.color = currentColor;
            }
        }
    }
}
