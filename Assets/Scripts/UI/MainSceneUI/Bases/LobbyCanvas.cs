using UI.MainSceneUI.Bottoms;
using UnityEngine;

namespace UI.MainSceneUI.Bases
{
    public class LobbyCanvas : MonoBehaviour, IMainSceneUIElement
    {
        [field: SerializeField] public ButtonsType MyButtonType { get; private set; }
        
        [SerializeField] private LobbyUI lobbyUI;
        public void Show()
        {
            lobbyUI.gameObject.SetActive(true);
            lobbyUI.Show();
        }

        public void Hide()
        {
            lobbyUI.Hide();
        }
    }
}