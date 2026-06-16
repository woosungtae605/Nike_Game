using System.Collections.Generic;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

namespace UI.MainSceneUI.Bottoms
{
    public class BottomCanvas : MonoBehaviour, IUIElement
    {
        [SerializeField] private GameObject[] mainSceneUIObjects;
        [SerializeField] private BottomButtonController bottom;
        [SerializeField] private RectTransform slidingBar;
        [SerializeField] private float slidingBarMoveDuration = 0.18f;

        private Dictionary<ButtonsType, IMainSceneUIElement> _mainSceneUIElementList = new();
        
        private bool _hasSelectedButton;
        private ButtonsType _nowButtonType;
        private MotionHandle _slidingBarMoveHandle;

        public void Awake()
        {
            foreach (GameObject obj in mainSceneUIObjects)
            {
                if (obj.TryGetComponent(out IMainSceneUIElement element))
                {
                    _mainSceneUIElementList[element.MyButtonType] = element;
                    element.Hide();
                }
            }

            if (bottom != null)
                bottom.OnClickButton += ClickEvent;
        }

        private void Start()
        {
            Canvas.ForceUpdateCanvases();
            SelectButton(ButtonsType.Lobby, true);
        }

        private void OnDestroy()
        {
            if (bottom != null)
                bottom.OnClickButton -= ClickEvent;

            if (_slidingBarMoveHandle.IsActive())
                _slidingBarMoveHandle.Cancel();
        }

        private void ClickEvent(ButtonsType obj)
        {
            SelectButton(obj, false);
        }

        private void SelectButton(ButtonsType obj, bool immediate)
        {
            if (_hasSelectedButton && _nowButtonType == obj)
                return;
            
            if (!_mainSceneUIElementList.TryGetValue(obj, out IMainSceneUIElement nextElement))
                return;

            MoveSlidingBar(obj, immediate);

            if (_hasSelectedButton && _mainSceneUIElementList.TryGetValue(_nowButtonType, out IMainSceneUIElement currentElement))
                currentElement.Hide();

            _nowButtonType = obj;
            _hasSelectedButton = true;
            nextElement.Show();
        }

        public void Hide()
        {
            if (bottom != null)
                bottom.OnClickButton -= ClickEvent;

            bottom.gameObject.SetActive(false);
        }

        private void MoveSlidingBar(ButtonsType buttonType, bool immediate = false)
        {
            if (slidingBar == null || bottom == null)
                return;

            if (!bottom.TryGetButtonRect(buttonType, out RectTransform buttonRect))
                return;

            float startX = slidingBar.position.x;
            float targetX = GetRectCenterWorldPosition(buttonRect).x;

            if (_slidingBarMoveHandle.IsActive())
                _slidingBarMoveHandle.Cancel();

            if (immediate)
            {
                SetSlidingBarWorldX(targetX);
                return;
            }

            _slidingBarMoveHandle = LMotion.Create(startX, targetX, slidingBarMoveDuration)
                .WithEase(Ease.OutCubic)
                .Bind(SetSlidingBarWorldX);
        }

        private void SetSlidingBarWorldX(float x)
        {
            Vector3 position = slidingBar.position;
            position.x = x;
            slidingBar.position = position;
        }

        private Vector3 GetRectCenterWorldPosition(RectTransform rectTransform)
        {
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            return (corners[0] + corners[2]) * 0.5f;
        }
    }
}
