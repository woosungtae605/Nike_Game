using UI.MainSceneUI.Bottoms;
using UnityEngine;

namespace UI.MainSceneUI
{
    public interface IMainSceneUIElement
    {
        public ButtonsType MyButtonType { get; }
        
        public void Show();
        public void Hide();
    }
}