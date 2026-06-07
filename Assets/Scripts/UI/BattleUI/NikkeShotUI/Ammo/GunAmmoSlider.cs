using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.BattleUI.NikkeShotUI.Ammo
{
    public class GunAmmoSlider : MonoBehaviour
    { 
        [SerializeField] private Image fillImage;

        [Header("Colors")]
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color dangerColor = Color.red;

        public event Action<bool> OnSliderDangerChanged;
        
        private Slider _slider;

        private void Awake()
        {
            _slider = GetComponent<Slider>();
        }
        
        public void SetAmmo(int currentAmmo, int maxAmmo)
        {
            if (_slider == null || maxAmmo <= 0)
            {
                Debug.LogError("ammoUI slider bug");
                return;
            }

            float value = Mathf.Clamp01((float)currentAmmo / maxAmmo);
            
            _slider.value = value;
            SetFillColor(value);
        }
        
        private void SetFillColor(float ratio)
        {
            if (fillImage == null)
                return;

            if (ratio <= 2f / 5f)
            {
                fillImage.color = dangerColor;
                OnSliderDangerChanged?.Invoke(true);
            }
            else
            {
                fillImage.color = normalColor;
                OnSliderDangerChanged?.Invoke(false);
            }
        }
    }
}