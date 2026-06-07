using System;
using TMPro;
using UnityEngine;

namespace UI.BattleUI.NikkeShotUI.Ammo
{
    public class GunAmmoTexts : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI[] ammoTexts;
        [SerializeField] private GameObject redPanel;

        public void SetAmmo(int currentAmmo)
        {
            currentAmmo = Mathf.Max(0, currentAmmo);

            for (int i = 0; i < ammoTexts.Length; i++)
            {
                if (ammoTexts[i] == null)
                    continue;

                int digit = currentAmmo / Pow10(i) % 10;
                ammoTexts[i].text = digit.ToString();
            }
        }

        private int Pow10(int power)
        {
            int value = 1;

            for (int i = 0; i < power; i++)
                value *= 10;

            return value;
        }

        public void PanelActive(bool value)
        {
            redPanel.SetActive(value);
        }
        
    }
}