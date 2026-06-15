using CoreSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.MainSceneUI.Bases
{
    public class LobbyUI : MonoBehaviour
    {
        [SerializeField] private Button battleStartButton;

        public void Show()
        {
            battleStartButton.onClick.AddListener(BattleStartButtonClick);
        }

        public void Hide()
        {
            battleStartButton.onClick.RemoveListener(BattleStartButtonClick);
            gameObject.SetActive(false);
        }

        private void BattleStartButtonClick()
        {
            FadeManager.Instance.FadeAndExecute(() => SceneManager.LoadScene("BattleStart"));
        }
    }
}