using System.Collections.Generic;
using UnityEngine;

namespace UI.MainSceneUI.Bottoms
{
    public class BottomCanvas : MonoBehaviour, IUIElement
    {
        [SerializeField] private GameObject[] mainSceneUIObjects;
        [SerializeField] private BottomButtonController bottom;

        private Dictionary<ButtonsType, IMainSceneUIElement> _mainSceneUIElementList = new();
        
        private bool _hasSelectedButton;
        private ButtonsType _nowButtonType;
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