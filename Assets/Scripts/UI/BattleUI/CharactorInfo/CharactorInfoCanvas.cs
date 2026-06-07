using System.Collections.Generic;
using Agents.Players;
using UnityEngine;

namespace UI.BattleUI.CharactorInfo
{
    public class CharactorInfoCanvas : MonoBehaviour, IUIElement<IReadOnlyList<Player>>
    {
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private CharactorInfo[] slots;

        private static readonly KeyCode[] KeyCodes =
        {
            KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R, KeyCode.T
        };

        private void Start()
        {
            Show(GetPlayers());
        }

        private void Update()
        {
            RefreshSlots();
        }

        public void Show(IReadOnlyList<Player> players)
        {
            if (slots == null)
                return;

            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] == null)
                    continue;

                KeyCode keyCode = i < KeyCodes.Length ? KeyCodes[i] : KeyCode.None;
                slots[i].SetKeyCode(keyCode);

                Player player = players != null && i < players.Count ? players[i] : null;
                slots[i].Show(player);
            }

            RefreshSlots();
        }

        public void Hide()
        {
            if (slots == null)
                return;

            foreach (CharactorInfo slot in slots)
            {
                if (slot == null)
                    continue;

                slot.Hide();
            }
        }

        private void RefreshSlots()
        {
            if (slots == null)
                return;

            Player currentPlayer = playerManager != null ? playerManager.CurrentPlayer : null;

            foreach (CharactorInfo slot in slots)
            {
                if (slot == null)
                    continue;

                slot.Refresh(currentPlayer);
            }
        }

        private IReadOnlyList<Player> GetPlayers()
        {
            if (playerManager == null)
                return System.Array.Empty<Player>();

            return playerManager.Players;
        }
    }
}
