using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Sound
{
    public class UISoundBinder : MonoBehaviour
    {
        [SerializeField] private AudioClip buttonClickClip;
        [SerializeField] private float sceneLoadBindDelay = 0.05f;

        private readonly HashSet<Button> _registeredButtons = new HashSet<Button>();
        private bool _isActiveBinder;
        private Coroutine _bindCoroutine;

        private void OnEnable()
        {
            SceneManager.sceneLoaded += HandleSceneLoaded;
            BindAfterSceneReady();
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;
            StopBindCoroutine();
            UnbindButtons();
        }

        private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (!_isActiveBinder)
                return;

            BindAfterSceneReady();
        }

        private void BindAfterSceneReady()
        {
            StopBindCoroutine();
            _bindCoroutine = StartCoroutine(BindAfterSceneReadyRoutine());
        }

        private IEnumerator BindAfterSceneReadyRoutine()
        {
            yield return null;

            if (sceneLoadBindDelay > 0f)
                yield return new WaitForSecondsRealtime(sceneLoadBindDelay);

            if (SoundManager.Instance == null || SoundManager.Instance.gameObject != gameObject)
                yield break;

            _isActiveBinder = true;
            BindButtons();
            _bindCoroutine = null;
        }

        private void BindButtons()
        {
            Button[] buttons = FindObjectsByType<Button>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            foreach (Button button in buttons)
            {
                if (button == null || _registeredButtons.Contains(button))
                    continue;

                button.onClick.RemoveListener(PlayButtonClickSound);
                button.onClick.AddListener(PlayButtonClickSound);
                _registeredButtons.Add(button);
            }
        }

        private void UnbindButtons()
        {
            foreach (Button button in _registeredButtons)
            {
                if (button != null)
                    button.onClick.RemoveListener(PlayButtonClickSound);
            }

            _registeredButtons.Clear();
            _isActiveBinder = false;
        }

        private void PlayButtonClickSound()
        {
            if (buttonClickClip == null)
                return;

            SoundManager.Instance?.PlaySFX(buttonClickClip);
        }

        private void StopBindCoroutine()
        {
            if (_bindCoroutine == null)
                return;

            StopCoroutine(_bindCoroutine);
            _bindCoroutine = null;
        }
    }
}
