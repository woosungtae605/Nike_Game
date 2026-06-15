using System;

namespace UI.MainSceneUI.Bottoms
{
    public interface IBottomButton
    {
        public event Action<ButtonsType> OnClick;
        public ButtonsType MyButtonType { get; }
    }
}