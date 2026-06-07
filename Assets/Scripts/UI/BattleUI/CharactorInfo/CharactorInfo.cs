using Agents.Module;
using Agents.Players;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.BattleUI.CharactorInfo
{
    public class CharactorInfo : MonoBehaviour, IUIElement<Player>
    {
        [SerializeField] private Image selectedBorder;
        [SerializeField] private Image dim;
        [SerializeField] private Image portrait;
        [SerializeField] private TextMeshProUGUI keyText;
        [SerializeField] private TextMeshProUGUI ammoText;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI noSignalText;
        [SerializeField] private RectTransform hpFill;
        [SerializeField] private RectTransform coverFill;
        [SerializeField] private Image statusDot;

        private Player _player;

        public void SetKeyCode(KeyCode keyCode)
        {
            if (keyText != null)
                keyText.text = keyCode.ToString();
        }

        public void Show(Player player)
        {
            _player = player;
            gameObject.SetActive(true);
            Refresh(null);
        }

        public void Hide()
        {
                    _player = null;
            gameObject.SetActive(false);
        }

public void Refresh(Player currentPlayer)
        {
            bool hasPlayer = _player != null;
            bool selected = hasPlayer && _player == currentPlayer;

            SetActive(selectedBorder, selected);
            SetActive(dim, false);
            SetActive(noSignalText, !hasPlayer);
            SetSlotContentActive(hasPlayer);

            if (!hasPlayer)
                return;

            if (statusDot != null)
                statusDot.color = selected ? new Color(1f, 0.73f, 0.15f, 1f) : new Color(0.24f, 0.42f, 0.68f, 1f);

            if (nameText != null)
                nameText.text = _player.name;

            UpdatePortrait();
            UpdateAmmo();
                    UpdateBar(hpFill, _player.HealthModule);
            UpdateBar(coverFill, _player.CoverModule);
        }

private void SetSlotContentActive(bool value)
        {
            SetActive(portrait, value);
            SetActive(keyText, value);
            SetActive(ammoText, value);
            SetActive(nameText, value);
            SetActive(GetBarRoot(hpFill), value);
            SetActive(GetBarRoot(coverFill), value);
            SetActive(statusDot, value);
        }

        private void UpdatePortrait()
        {
            if (portrait == null)
                return;

            if (_player.PlayerData != null && _player.PlayerData.NikkeSprite != null)
                portrait.sprite = _player.PlayerData.NikkeSprite;
        }

        private void UpdateAmmo()
        {
            if (ammoText == null)
                return;

            if (_player.PlayerGunCompo == null)
            {
                ammoText.text = "--/--";
                return;
            }

            ammoText.SetText("{0}/{1}", _player.PlayerGunCompo.CurrentAmmo, _player.PlayerGunCompo.GunData.MaxAmmo);
        }

        private static Component GetBarRoot(RectTransform fill)
                {
            if (fill == null)
                return null;

            return fill.parent != null ? fill.parent : fill;
        }

        private static void UpdateBar(RectTransform fill, IBar bar)
        {
            if (fill == null)
                return;

            float ratio = 0f;
            if (bar != null && bar.MaxValue > 0)
                ratio = Mathf.Clamp01((float)bar.CurrentValue / bar.MaxValue);

            fill.anchorMin = new Vector2(0f, fill.anchorMin.y);
            fill.anchorMax = new Vector2(ratio, fill.anchorMax.y);
            fill.offsetMin = new Vector2(0f, fill.offsetMin.y);
            fill.offsetMax = new Vector2(0f, fill.offsetMax.y);
        }

        private static void SetActive(Component component, bool value)
        {
            if (component != null)
                component.gameObject.SetActive(value);
        }
    }
}
