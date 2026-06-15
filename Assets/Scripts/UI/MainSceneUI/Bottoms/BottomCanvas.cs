using System.Collections.Generic;
using UnityEngine;

namespace UI.MainSceneUI.Bottoms
{
    public class BottomCanvas : MonoBehaviour, IUIElement
    {
        [SerializeField] private MonoBehaviour[] mainSceneUIElements;
        [SerializeField] private BottomButtonController bottom;

        private Dictionary<ButtonsType, IMainSceneUIElement> _mainSceneUIElementList = new();
        
        private bool _hasSelectedButton;
        private ButtonsType _nowButtonType;
        public void Awake()
        {
            foreach (MonoBehaviour mainSceneUIElement in mainSceneUIElements)
            {
                if (mainSceneUIElement is IMainSceneUIElement iMainSceneUIElement)
                {
                    _mainSceneUIElementList[iMainSceneUIElement.MyButtonType] = iMainSceneUIElement;
                    iMainSceneUIElement.Hide();
                }
            }
            ClickEvent(ButtonsType.Lobby);
            bottom.OnClickButton += ClickEvent;
        }

        private void OnDestroy()
        {
            bottom.OnClickButton -= ClickEvent;
        }

        private void ClickEvent(ButtonsType obj)
        {
            if (_hasSelectedButton && _nowButtonType == obj)
                return;
            
            if (!_mainSceneUIElementList.TryGetValue(obj, out IMainSceneUIElement nextElement))
                return;

            if (_hasSelectedButton && _mainSceneUIElementList.TryGetValue(_nowButtonType, out IMainSceneUIElement currentElement))
                currentElement.Hide();

            _nowButtonType = obj;
            _hasSelectedButton = true;
            nextElement.Show();
        }

        public void Hide()
        {
            bottom.OnClickButton -= ClickEvent;
            bottom.gameObject.SetActive(false);
        }
    }
}