using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainSceneUI.Bottoms
{
    public class BottomButton : MonoBehaviour, IBottomButton
    {
        [field: SerializeField] public ButtonsType MyButtonType { get; private set; }
        
        public event Action<ButtonsType> OnClick;
        private Button _button;
        
        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            _button = GetComponent<Button>();
            
            if(_button != null)
                _button.onClick.AddListener(ClickEvent);
        }

        private void OnDestroy()
        {
            if(_button != null)
                _button.onClick.RemoveListener(ClickEvent);
        }

        private void ClickEvent()
        {
            OnClick?.Invoke(MyButtonType);
        }
    }
}