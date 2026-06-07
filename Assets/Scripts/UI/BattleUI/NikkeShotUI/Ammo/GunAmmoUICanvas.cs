using CoreSystem.BusSystem;
using GameEvents.UI;
using UnityEngine;

namespace UI.BattleUI.NikkeShotUI.Ammo
{
    public class GunAmmoUICanvas : MonoBehaviour, IUIElement<GunAmmoUIActiveEvent>
    {
        [SerializeField] private GameObject ammoObject;
        private UIFollowMouse _uiFollowMouse;
        private GunAmmoSlider _gunAmmoSlider;
        private GunAmmoTexts _ammoTexts;
        private void Awake()
        {
            _uiFollowMouse = ammoObject.GetComponent<UIFollowMouse>();
            _gunAmmoSlider = GetComponentInChildren<GunAmmoSlider>(true);
            _ammoTexts = GetComponentInChildren<GunAmmoTexts>(true);

            _gunAmmoSlider.OnSliderDangerChanged += HandleSliderDangerChanged;
            
            Bus<GunAmmoUIActiveEvent>.OnEvent += HandleAmmoUIActive; 
            
            ammoObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _gunAmmoSlider.OnSliderDangerChanged -= HandleSliderDangerChanged;
            
            Bus<GunAmmoUIActiveEvent>.OnEvent -= HandleAmmoUIActive;
        }
        
        private void HandleSliderDangerChanged(bool value)
        {
            _ammoTexts.PanelActive(value);
        }

        private void HandleAmmoUIActive(GunAmmoUIActiveEvent obj)
        {
            if (obj.Active)
                Show(obj);
            else
                Hide();
        }

        public void Show(GunAmmoUIActiveEvent item)
        {
            ammoObject.SetActive(true);
            _uiFollowMouse.SnapToCurrentMousePosition();
            _gunAmmoSlider.SetAmmo(item.CurrentAmmo, item.MaxAmmo);
            _ammoTexts.SetAmmo(item.CurrentAmmo);
        }

        public void Hide()
        {
            ammoObject.SetActive(false);
        }
    }
}