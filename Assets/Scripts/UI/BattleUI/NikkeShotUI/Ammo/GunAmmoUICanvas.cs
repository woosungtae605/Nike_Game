using CoreSystem.BusSystem;
using GameEvents.UI;
using UnityEngine;

namespace UI.BattleUI.NikkeShotUI.Ammo
{
    public class GunAmmoUICanvas : MonoBehaviour, IUIElement<GunAmmoUIActiveEvent>
    {
        [SerializeField] private GameObject ammoObject;
        private GunAmmoSlider _gunAmmoSlider;
        private GunAmmoTexts _ammoTexts;
        private void Awake()
        {
            _gunAmmoSlider = GetComponentInChildren<GunAmmoSlider>(true);
            _ammoTexts = GetComponentInChildren<GunAmmoTexts>(true);
            Bus<GunAmmoUIActiveEvent>.OnEvent += Show;

            if (ammoObject.activeSelf)
                ammoObject.SetActive(false);
        }

        private void OnDestroy()
        {
            Bus<GunAmmoUIActiveEvent>.OnEvent -= Show;
        }

        public void Show(GunAmmoUIActiveEvent item)
        {
            ammoObject.SetActive(true);
        }

        public void Hide()
        {
            ammoObject.SetActive(false);
        }
    }
}