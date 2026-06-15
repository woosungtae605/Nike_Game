using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.MainSceneUI.Bottoms
{
    public class BottomButtonController : MonoBehaviour
    {
        private List<IBottomButton>  _bottomButtons;

        public event Action<ButtonsType> OnClickButton;

        private void Awake()
        {
            _bottomButtons = GetComponentsInChildren<IBottomButton>().ToList();

            foreach (IBottomButton button in _bottomButtons)
            {
                button.OnClick += ClickEvent;
            }
        }

        private void OnDestroy()
        {
            foreach (IBottomButton button in _bottomButtons)
            {
                button.OnClick -= ClickEvent;
            }
        }

        private void ClickEvent(ButtonsType obj)
        {
            OnClickButton?.Invoke(obj);
        }
    }
}