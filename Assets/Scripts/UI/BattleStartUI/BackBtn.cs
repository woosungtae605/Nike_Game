using CoreSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.BattleStartUI
{
    public class BackBtn : MonoBehaviour
    {
        [SerializeField] private GameObject objec;
        public void BackBtnClick()
        {
            FadeManager.Instance.FadeAndExecute(() => SceneManager.LoadScene("MainScene"));
        }

        public void ActiveFalse()
        {
            objec.gameObject.SetActive(false);
        }
    }
}