using System;
using CoreSystem;
using Systems.SaveSystem;
using Sound;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.EndingUI
{
    public class EndingSceneController : MonoBehaviour
    {
        [SerializeField] private SaveFileNameSO endingSaveFile;
        [SerializeField] private AudioClip endingBgm;
        [SerializeField] private string nextSceneName = "MainScene";
        [SerializeField] private string titleText = "모든 씬을 클리어 했습니다";
        [SerializeField] private string subText = "버튼을 눌러 메인으로 돌아갑니다";
        [SerializeField, TextArea(5, 12)] private string scrollText = @"냥케

플레이해주셔서 감사합니다.

당신의 승리로
모든 전투가 끝났습니다.

THE END";
        [SerializeField] private float inputDelay = 1f;
        [SerializeField] private float scrollStartYRatio = 1.08f;
        [SerializeField] private float scrollSpeed = 34f;
        [SerializeField] private float scrollTextWidthRatio = 0.82f;

        private float _startTime;
        private float _canCompleteTime;
        private bool _completed;

        private void Awake()
        {
            _startTime = Time.unscaledTime;
            _canCompleteTime = Time.unscaledTime + inputDelay;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            PlayEndingBgm();
        }


        private void OnGUI()
        {
            DrawBackground();
            DrawScrollingText();

            if (Time.unscaledTime >= _canCompleteTime)
                DrawGuideButton();
        }

        private void DrawBackground()
        {
            GUI.color = Color.white;
        }

        private void DrawScrollingText()
        {
            GUIStyle scrollStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.UpperCenter,
                fontSize = Mathf.Max(22, Screen.height / 32),
                fontStyle = FontStyle.Bold,
                wordWrap = true,
                normal = { textColor = Color.white }
            };

            GUIStyle titleStyle = new GUIStyle(scrollStyle)
            {
                fontSize = Mathf.Max(34, Screen.height / 18)
            };

            float elapsed = Time.unscaledTime - _startTime;
            float startY = Screen.height * scrollStartYRatio;
            float y = startY - elapsed * scrollSpeed;
            float width = Screen.width * scrollTextWidthRatio;
            float x = (Screen.width - width) * 0.5f;

            GUI.Label(new Rect(0f, y - 110f, Screen.width, 80f), titleText, titleStyle);
            GUI.Label(new Rect(x, y, width, Screen.height * 2f), scrollText, scrollStyle);
        }

        private void DrawGuideButton()
        {
            GUIStyle subStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = Mathf.Max(15, Screen.height / 48),
                normal = { textColor = new Color(0.72f, 0.72f, 0.72f, 1f) }
            };

            GUI.Label(new Rect(0f, Screen.height - 92f, Screen.width, 28f), subText, subStyle);

            Rect buttonRect = new Rect(Screen.width * 0.5f - 100f, Screen.height - 58f, 200f, 40f);
            if (GUI.Button(buttonRect, "MAIN"))
                CompleteEnding();
        }



        private void PlayEndingBgm()
        {
            if (endingBgm == null)
                return;

            SoundManager.Instance?.PlayBGM(endingBgm);
        }
        private void CompleteEnding()
        {
            if (_completed)
                return;

            _completed = true;
            EndingClearSave.MarkEndingViewed(endingSaveFile);

            if (FadeManager.Instance != null)
                FadeManager.Instance.FadeAndExecute(() => SceneManager.LoadScene(nextSceneName));
            else
                SceneManager.LoadScene(nextSceneName);
        }
    }

    [Serializable]
    public class EndingClearSaveData
    {
        public bool isEndingViewed;
    }

    public static class EndingClearSave
    {
        private static EndingClearSaveData _cache;
        private static SaveFileNameSO _cacheSaveFile;

        public static bool IsEndingViewed(SaveFileNameSO saveFile)
        {
            if (saveFile == null)
                return false;

            EndingClearSaveData data = Load(saveFile);
            return data.isEndingViewed;
        }

        public static void MarkEndingViewed(SaveFileNameSO saveFile)
        {
            if (saveFile == null)
                return;

            EndingClearSaveData data = Load(saveFile);
            if (data.isEndingViewed)
                return;

            data.isEndingViewed = true;
            _cache = data;
            _cacheSaveFile = saveFile;
            JsonSaveService.Save(saveFile, data);
        }

        private static EndingClearSaveData Load(SaveFileNameSO saveFile)
        {
            if (_cache != null && _cacheSaveFile == saveFile)
                return _cache;

            _cacheSaveFile = saveFile;

            if (!JsonSaveService.TryLoad(saveFile, out _cache) || _cache == null)
                _cache = new EndingClearSaveData();

            return _cache;
        }
    }
}


