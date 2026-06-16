using Agents.Players;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainSceneUI.Nikkes.NikkeProfiles
{
    public class NikkeProfile : MonoBehaviour, IUIElement<PlayerDataSO>
    {
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Image profileImage;
        [SerializeField] private Button clickBtn;
        
        public void Show(PlayerDataSO item)
        {
            
        }

        public void Hide()
        {
            
        }
    }
}