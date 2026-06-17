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
        [SerializeField] private NikkeInformationUI nikkeInformationUI;
        [SerializeField] private PlayerDataSos playerDataSos;

        private void Awake()
        {
            if (nikkeContainer != null)
                nikkeContainer.OnClickProfile += HandleClickProfile;

            if (nikkeInformationUI != null)
                nikkeInformationUI.OnUpgrade += HandleUpgrade;
        }

        private void OnDestroy()
        {
            if (nikkeContainer != null)
                nikkeContainer.OnClickProfile -= HandleClickProfile;

            if (nikkeInformationUI != null)
                nikkeInformationUI.OnUpgrade -= HandleUpgrade;
        }

        public void Show()
        {
            nikkeUI.gameObject.SetActive(true);
            nikkeContainer.Init(playerDataSos);
        }

        public void Hide()
        {
            nikkeUI.gameObject.SetActive(false);
        }

        private void HandleClickProfile(PlayerDataSO playerData)
        {
            if (nikkeInformationUI == null)
                return;

            nikkeInformationUI.Show(playerData);
        }

        private void HandleUpgrade(PlayerDataSO playerData)
        {
            if (nikkeContainer != null)
                nikkeContainer.RefreshProfileLevels();
        }
    }
}
