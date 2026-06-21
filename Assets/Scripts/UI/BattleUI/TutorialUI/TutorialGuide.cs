using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.BattleUI.TutorialUI
{
    public class TutorialGuide : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI tutorialText;
        [SerializeField] protected Image characterImage;
        public void TutorialGo(string text, Sprite sprite)
        {
            characterImage.sprite = sprite;
            tutorialText.text = text;
            Time.timeScale = 0;
        }
    }
}