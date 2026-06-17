using CoreSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.BattleStartUI
{
    public class BackBtn : MonoBehaviour
    {
        public void BackBtnClick()
        {
            FadeManager.Instance.FadeAndExecute(() => SceneManager.LoadScene("MainScene"));
        }
    }
}