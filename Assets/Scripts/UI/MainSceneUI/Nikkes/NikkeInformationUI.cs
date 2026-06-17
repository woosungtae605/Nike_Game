using Agents.Players;
using Systems.UpgradeSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainSceneUI.Nikkes
{
    public class NikkeInformationUI : MonoBehaviour
    {
        [SerializeField] private UpgradeManager upgradeManager;
        
        [SerializeField] private GameObject showGameObject;

        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Image nikkeImage;
        
        [Header("Status")] 
        [SerializeField] private TextMeshProUGUI attackText;
        [SerializeField] private TextMeshProUGUI hpText;
        [SerializeField] private TextMeshProUGUI levelText;
        
        [Header("Details")] 
        [SerializeField] private TextMeshProUGUI sexualityText;
        [SerializeField] private TextMeshProUGUI ageText;
        [SerializeField] private TextMeshProUGUI gunText;
        [SerializeField] private TextMeshProUGUI storyText;
        [SerializeField] private TextMeshProUGUI needMoney;

        [SerializeField] private Button upgradeBtn;
        
        PlayerDataSO _playerData;
        public event System.Action<PlayerDataSO> OnUpgrade;
        
        private void Awake()
        {
            upgradeBtn.onClick.AddListener(Upgrade);
        }

        private void OnDestroy()
        {
            upgradeBtn.onClick.RemoveListener(Upgrade);
        }

        private void Upgrade()
        {
            if (_playerData == null || upgradeManager == null)
                return;

            if (!upgradeManager.TryUpgrade(_playerData))
                return;

            Refresh();
            OnUpgrade?.Invoke(_playerData);
        }
        
        private void Refresh()
        {
            if (_playerData == null)
                return;

            int level = upgradeManager != null ? upgradeManager.GetLevel(_playerData) : 0;
            int maxLevel = upgradeManager != null ? upgradeManager.MaxLevel : 0;
            bool isMaxLevel = level >= maxLevel;

            attackText.text = upgradeManager != null
                ? upgradeManager.GetAttack(_playerData).ToString()
                : GetBaseAttack(_playerData).ToString();
            hpText.text = upgradeManager != null
                ? upgradeManager.GetMaxHp(_playerData).ToString()
                : _playerData.MaxHp.ToString();

            levelText.text = $"{level + 1} / {maxLevel + 1}";
            needMoney.text = isMaxLevel || upgradeManager == null
                ? "MAX"
                : upgradeManager.GetUpgradeCost(_playerData).ToString();

            if (upgradeBtn != null)
                upgradeBtn.interactable = upgradeManager != null && !isMaxLevel;
        }
        
        public void Show(PlayerDataSO playerDataSo)
        {
            if (playerDataSo == null)
                return;

            _playerData = playerDataSo;
            showGameObject.SetActive(true);

            sexualityText.text = playerDataSo.Sexuality;
            ageText.text = playerDataSo.Age.ToString();
            nameText.text = playerDataSo.NikkeName;
            gunText.text = playerDataSo.PlayerGunData != null && playerDataSo.PlayerGunData.GunData != null
                ? playerDataSo.PlayerGunData.GunData.GunName
                : string.Empty;
            storyText.text = playerDataSo.NikkeDescription;
            nikkeImage.sprite = playerDataSo.NikkeSprite;
            Refresh();
        }

        public void Hide()
        {
            _playerData = null;
            showGameObject.SetActive(false);
        }

        private int GetBaseAttack(PlayerDataSO playerData)
        {
            return playerData.PlayerGunData != null && playerData.PlayerGunData.GunData != null
                ? playerData.PlayerGunData.GunData.Damage
                : 0;
        }
    }
}
