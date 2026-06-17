using System;
using Agents.Players;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.MainSceneUI.Squards
{
    public class SquadSlotUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private Sprite noEquip;

        private PlayerDataSO _playerData;
        private int _index;
        public event Action<int> OnClickSlot;

        public void Init(int index, Sprite emptySprite)
        {
            _index = index;
            noEquip = emptySprite;

            if (iconImage == null)
                iconImage = GetComponent<Image>();
        }

        public void SetPlayer(PlayerDataSO playerData)
        {
            _playerData = playerData;

            if (iconImage == null)
                return;

            iconImage.sprite = _playerData != null ? _playerData.NikkeSprite : noEquip;
            Color color = iconImage.color;
            color.a = _playerData != null ? 1f : 0f;
            iconImage.color = color;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_playerData == null)
                return;

            OnClickSlot?.Invoke(_index);
        }
    }
}
