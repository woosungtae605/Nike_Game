using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI.MainSceneUI.Bottoms
{
    public class BottomButtonController : MonoBehaviour
    {
        private List<IBottomButton> _bottomButtons;

        public event Action<ButtonsType> OnClickButton;

        private void Awake()
        {
            EnsureButtons();

            foreach (IBottomButton button in _bottomButtons)
            {
                button.OnClick += ClickEvent;
            }
        }

        private void OnDestroy()
        {
            if (_bottomButtons == null)
                return;

            foreach (IBottomButton button in _bottomButtons)
            {
                button.OnClick -= ClickEvent;
            }
        }

        private void ClickEvent(ButtonsType obj)
        {
            OnClickButton?.Invoke(obj);
        }

        public bool TryGetButtonRect(ButtonsType buttonType, out RectTransform rectTransform)
        {
            rectTransform = null;
            EnsureButtons();

            foreach (IBottomButton button in _bottomButtons)
            {
                if (button.MyButtonType != buttonType)
                    continue;

                if (button is Component component)
                    rectTransform = component.transform as RectTransform;

                return rectTransform != null;
            }

            return false;
        }

        private void EnsureButtons()
        {
            if (_bottomButtons != null)
                return;

            _bottomButtons = GetComponentsInChildren<IBottomButton>().ToList();
        }
    }
}
