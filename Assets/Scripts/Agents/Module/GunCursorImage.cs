using LitMotion;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Agents.Module
{
    public class GunCursorImage : MonoBehaviour
    {
        [Header("Scale Motion")]
        [SerializeField] private float scalePower = 1.2f;
        [SerializeField] private float scaleDuration = 0.08f;
        
        private UIFollowMouse _uiFollowMouse;
        private Vector3 _originScale;
        private MotionHandle _scaleHandle;

        public void Init()
        {
            _uiFollowMouse = GetComponent<UIFollowMouse>();
            _originScale = transform.localScale;
        }

        public void SetActiveFalse()
        {
            _scaleHandle.TryCancel();
            gameObject.SetActive(false);
        }

        public void ActiveTrue()
        {
            _uiFollowMouse?.SnapToCurrentMousePosition();
        }
        private void OnDestroy()
        {
            _scaleHandle.TryCancel();
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
    }
}