using Agents.Players;
using UI.MainSceneUI.Bottoms;
using UI.MainSceneUI.Nikkes;
using UnityEngine;

namespace UI.MainSceneUI.Squards
{
    public class SquadCanvas : MonoBehaviour, IMainSceneUIElement
    {
        [field: SerializeField] public ButtonsType MyButtonType { get; private set; }
        [SerializeField] private PlayerDataSos playerDataSos;
        [SerializeField] private NikkeContainer nikkeContainer;
        [SerializeField] private GameObject squad;
        public void Show()
        {
            squad.SetActive(true);
            nikkeContainer.Init(playerDataSos);
        }

        public void Hide()
        {
            squad.SetActive(false);
        }
    }
}