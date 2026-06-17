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
        [SerializeField] private PlayerSquadSO playerSquad;
        [SerializeField] private NikkeContainer nikkeContainer;
        [SerializeField] private SquadPlusUI squadPlusUI;
        [SerializeField] private GameObject squad;

        private bool _isSubscribed;

        public void Show()
        {
            squad.SetActive(true);
            SubscribeContainer();
            nikkeContainer.Init(playerDataSos);

            if (squadPlusUI != null)
                squadPlusUI.Init();
        }

        public void Hide()
        {
            UnsubscribeContainer();
            squad.SetActive(false);
        }

        private void OnDestroy()
        {
            UnsubscribeContainer();
        }

        private void SubscribeContainer()
        {
            if (_isSubscribed || nikkeContainer == null)
                return;

            nikkeContainer.OnClickProfile += HandleClickProfile;
            _isSubscribed = true;
        }

        private void UnsubscribeContainer()
        {
            if (!_isSubscribed || nikkeContainer == null)
                return;

            nikkeContainer.OnClickProfile -= HandleClickProfile;
            _isSubscribed = false;
        }

        private void HandleClickProfile(PlayerDataSO playerData)
        {
            if (playerSquad == null)
                return;

            if (!playerSquad.Equip(playerData))
                return;

            if (squadPlusUI != null)
                squadPlusUI.Init();
        }
    }
}
