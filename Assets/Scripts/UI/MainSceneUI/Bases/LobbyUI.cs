using Agents.Players;
using CoreSystem;
using UI.MainSceneUI.Tutorial;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.MainSceneUI.Bases
{
    public class LobbyUI : MonoBehaviour
    {
        [SerializeField] private Button battleStartButton;
        [SerializeField] private PlayerSquadSO playerSquad;
        [SerializeField] private MainSceneTutorialGuide warningGuide;
        [SerializeField] private string emptySquadWarningText = "스쿼드를 장착해야 합니다.";

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
            if (playerSquad == null || !playerSquad.HasEquippedPlayer())
            {
                if (warningGuide != null)
                    warningGuide.ShowWarning(emptySquadWarningText);
                else
                    Debug.LogWarning(emptySquadWarningText, this);

                return;
            }

            FadeManager.Instance.FadeAndExecute(() => SceneManager.LoadScene("BattleStart"));
        }
    }
}
