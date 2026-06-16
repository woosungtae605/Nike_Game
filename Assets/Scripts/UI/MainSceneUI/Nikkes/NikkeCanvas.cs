using Agents.Players;
using UI.MainSceneUI.Bottoms;
using UnityEngine;

namespace UI.MainSceneUI.Nikkes
{
    public class NikkeCanvas : MonoBehaviour, IMainSceneUIElement
    {
        [field: SerializeField] public ButtonsType MyButtonType { get; private set; }
        [SerializeField] private GameObject nikkeUI;
        [SerializeField] private NikkeContainer nikkeContainer;
        [SerializeField] private PlayerDataSos playerDataSos;
        public void Show()
        {
            nikkeUI.gameObject.SetActive(true);
            nikkeContainer.Init(playerDataSos);
        }

        public void Hide()
        {
            nikkeUI.gameObject.SetActive(false);
        }
    }
}