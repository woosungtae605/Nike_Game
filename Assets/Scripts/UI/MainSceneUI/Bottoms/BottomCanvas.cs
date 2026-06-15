using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.MainSceneUI.Bottoms
{
    public class BottomCanvas : MonoBehaviour, IUIElement
    {
        [SerializeField] private BottomButtonController bottom;

        public void Awake()
        {
            bottom.OnClickButton += ClickEvent;
        }

        private void OnDestroy()
        {
            bottom.OnClickButton -= ClickEvent;
        }

        private void ClickEvent(ButtonsType obj)
        {
            
        }

        public void Hide()
        {
            bottom.OnClickButton -= ClickEvent;
            bottom.gameObject.SetActive(false);
        }
    }
}