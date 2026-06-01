using CoreSystem;
using CoreSystem.BusSystem;
using GameEvents;
using GameEvents.UI;
using TMPro;
using UnityEngine;

namespace UI.BattleUI.NikkeShotUI
{
    public class GunAmmoUI : MonoBehaviour, IUIElement<(int currentAmmo, int maxAmmo)>
    {
        private TextMeshProUGUI _ammoText;

        private void Awake()
        {
            _ammoText = GetComponentInChildren<TextMeshProUGUI>(true);
            _ammoText.gameObject.SetActive(false);
            Bus<GunAmmoUIActiveEvent>.OnEvent += HandleGunAmmoUIActive;
        }

        private void OnDestroy()
        {
            Bus<GunAmmoUIActiveEvent>.OnEvent -= HandleGunAmmoUIActive;
        }

        private void HandleGunAmmoUIActive(GunAmmoUIActiveEvent obj)
        {
            Show((obj.CurrentAmmo, obj.MaxAmmo));
        }

        public void Show((int currentAmmo, int maxAmmo) item)
        {
            _ammoText.gameObject.SetActive(true);   
            _ammoText.text = $"{item.currentAmmo} / {item.maxAmmo}";
        }

        public void Hide()
        {
            _ammoText.gameObject.SetActive(false);
        }
    }
}