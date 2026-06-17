using Agents.Players;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainSceneUI.Nikkes
{
    public class NikkeInformationUI : MonoBehaviour
    {
        [SerializeField] private GameObject showGameObject;

        [Header("Status")] 
        [SerializeField] private TextMeshProUGUI attackText;
        [SerializeField] private TextMeshProUGUI hpText;

        [Header("Details")] 
        [SerializeField] private TextMeshProUGUI sexualityText;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI gunText;
        [SerializeField] private TextMeshProUGUI storyText;

        [SerializeField] private Button upgradeBtn;
        
        PlayerDataSO _playerData;
        public void Show(PlayerDataSO playerDataSo)
        {
            _playerData = playerDataSo;
            showGameObject.SetActive(true);

            attackText.text = playerDataSo.PlayerGunData.GunData.Damage.ToString();
            hpText.text = playerDataSo.MaxHp.ToString();
            sexualityText.text = playerDataSo.Sexuality;
            nameText.text = playerDataSo.NikkeName;
            gunText.text = playerDataSo.PlayerGunData.GunData.GunName;
            storyText.text = playerDataSo.NikkeDescription;
        }
    }
}