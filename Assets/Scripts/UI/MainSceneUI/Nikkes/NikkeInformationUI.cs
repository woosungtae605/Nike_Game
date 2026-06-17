using Agents.Players;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainSceneUI.Nikkes
{
    public class NikkeInformationUI : MonoBehaviour
    {
        [SerializeField] private GameObject showGameObject;

        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Image nikkeImage;
        
        [Header("Status")] 
        [SerializeField] private TextMeshProUGUI attackText;
        [SerializeField] private TextMeshProUGUI hpText;

        [Header("Details")] 
        [SerializeField] private TextMeshProUGUI sexualityText;
        [SerializeField] private TextMeshProUGUI ageText;
        [SerializeField] private TextMeshProUGUI gunText;
        [SerializeField] private TextMeshProUGUI storyText;
        [SerializeField] private TextMeshProUGUI needMoney;

        [SerializeField] private Button upgradeBtn;
        
        PlayerDataSO _playerData;
        public void Show(PlayerDataSO playerDataSo)
        {
            if (playerDataSo == null)
                return;

            _playerData = playerDataSo;
            showGameObject.SetActive(true);

            attackText.text = playerDataSo.PlayerGunData != null && playerDataSo.PlayerGunData.GunData != null
                ? playerDataSo.PlayerGunData.GunData.Damage.ToString() : "0";
            hpText.text = playerDataSo.MaxHp.ToString();
            sexualityText.text = playerDataSo.Sexuality;
            ageText.text = playerDataSo.Age.ToString();
            nameText.text = playerDataSo.NikkeName;
            gunText.text = playerDataSo.PlayerGunData != null && playerDataSo.PlayerGunData.GunData != null
                ? playerDataSo.PlayerGunData.GunData.GunName
                : string.Empty;
            storyText.text = playerDataSo.NikkeDescription;
            nikkeImage.sprite = playerDataSo.NikkeSprite;
        }

        public void Hide()
        {
            _playerData = null;
            showGameObject.SetActive(false);
        }
    }
}
