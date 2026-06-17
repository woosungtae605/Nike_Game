using Agents.Players;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainSceneUI.Squards
{
    public class SquadPlusUI : MonoBehaviour
    {
        [SerializeField] private PlayerSquadSO playerSquad;
        [SerializeField] private Image[] images = new Image[5];
        [SerializeField] private SquadSlotUI[] slots = new SquadSlotUI[5];
        [SerializeField] private Sprite noEquip;

        private void OnEnable()
        {
            SetupSlots();

            if (playerSquad != null)
                playerSquad.OnChanged += Init;

            Init();
        }

        private void OnDisable()
        {
            if (playerSquad != null)
                playerSquad.OnChanged -= Init;

            UnsubscribeSlots();
        }

        public void Init()
        {
            if (playerSquad == null)
                return;

            for (int i = 0; i < playerSquad.PlayerDataSos.Length; i++)
            {
                SquadSlotUI slot = GetSlot(i);
                if (slot == null)
                    continue;

                slot.SetPlayer(playerSquad.PlayerDataSos[i]);
            }
        }

        private void SetupSlots()
        {
            if (images == null)
                return;

            if (slots == null || slots.Length != images.Length)
                slots = new SquadSlotUI[images.Length];

            for (int i = 0; i < images.Length; i++)
            {
                if (images[i] == null)
                    continue;

                if (slots[i] == null)
                    slots[i] = images[i].GetComponent<SquadSlotUI>();

                if (slots[i] == null)
                    slots[i] = images[i].gameObject.AddComponent<SquadSlotUI>();

                slots[i].OnClickSlot -= HandleClickSlot;
                slots[i].OnClickSlot += HandleClickSlot;
                slots[i].Init(i, noEquip);
            }
        }

        private void UnsubscribeSlots()
        {
            if (slots == null)
                return;

            foreach (SquadSlotUI slot in slots)
            {
                if (slot != null)
                    slot.OnClickSlot -= HandleClickSlot;
            }
        }

        private void HandleClickSlot(int index)
        {
            if (playerSquad == null)
                return;

            playerSquad.Unequip(index);
        }

        private SquadSlotUI GetSlot(int index)
        {
            if (slots == null || index < 0 || index >= slots.Length)
                return null;

            return slots[index];
        }
    }
}
