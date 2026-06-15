using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CoreSystem
{
    public class FadeManager : MonoBehaviour
    {
        public static FadeManager Instance { get; private set; }
    
        [SerializeField] private Image fadeImage;
        [SerializeField] private float fadeDuration = 1f;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void FadeAndExecute(Action action)
        {
            StartCoroutine(FadeCoroutine(action));
        }

        private IEnumerator FadeCoroutine(Action action)
        {
            yield return StartCoroutine(Fade(1f));
            action?.Invoke();
            yield return StartCoroutine(Fade(0f));
        }

        private IEnumerator Fade(float targetAlpha)
        {
            float startAlpha = fadeImage.color.a;
            float elapsed = 0f;

            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
                fadeImage.color = new Color(0, 0, 0, alpha);
                yield return null;
            }

            fadeImage.color = new Color(0, 0, 0, targetAlpha);
        }
    }
}