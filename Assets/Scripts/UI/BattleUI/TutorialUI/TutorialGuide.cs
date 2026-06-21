using System;
using System.Collections.Generic;
using Agents.Enemies;
using CoreSystem.BusSystem;
using GameEvents.UI;
using Systems;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI.BattleUI.TutorialUI
{
    public class TutorialGuide : MonoBehaviour
    {
        public event Action OnTutorialCompleted;

        [SerializeField] private PlayerInputSO playerInput;
        [SerializeField] private TextMeshProUGUI tutorialText;
        [SerializeField] protected Image characterImage;
        [SerializeField] private GameObject guideRoot;
        [SerializeField] private float clickDelay = 1f;

        [Header("Sprites")]
        [SerializeField] private Sprite lukaSprite;
        [SerializeField] private Sprite catSprite;

        [Header("Intro")]
        [SerializeField] private string catIntroText1 = "애옭.";
        [SerializeField] private string catIntroText2 = "애옭!!";
        [SerializeField] private string catIntroText3 = "애옭!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!";
        [SerializeField] private string lukaIntroText = "야 뚱냥이년아 잠깐 비켜.";

        [Header("Enemy Spawn")]
        [SerializeField] private string enemySpawnText = "저기 적이 나타났어.";
        [SerializeField] private string attackGuideText = "적을 공격해봐. 적을 공격하는 방법은 오른쪽 클릭으로 조준 한다음 왼쪽클릭으로 총을 발사하면 돼.";
        [SerializeField] private string enemyDeathText = "잘했어. 그럼 한번 계속 해봐.";
        [SerializeField] private string bossSpawnText = "랩처를 처치하며 전투를 진행하다 보면 무리를 이끄는 고위급 개체가 등장해. 이 타겟을 처치하면 게임에서 승리하게 돼.";

        private readonly Queue<TutorialLine> _lineQueue = new();
        private AbstractEnemy _tutorialEnemy;
        private bool _isPlaying;
        private bool _showAttackGuideAfterEnemySpawn;
        private bool _hasPlayedIntro;
        private float _canCompleteTime;

        private void Awake()
        {
            if (guideRoot == null)
                guideRoot = gameObject;

            Bus<TutorialEnemySpawnEvent>.OnEvent += HandleTutorialEnemySpawn;
            Bus<TutorialBossSpawnEvent>.OnEvent += HandleTutorialBossSpawn;
            HideImmediate();
        }

        private void Update()
        {
            if (!_isPlaying)
                return;

            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
                TryCompleteTutorial();
        }

        private void OnDestroy()
        {
            Bus<TutorialEnemySpawnEvent>.OnEvent -= HandleTutorialEnemySpawn;
            Bus<TutorialBossSpawnEvent>.OnEvent -= HandleTutorialBossSpawn;
            UnbindTutorialEnemy();
            UnbindInput();
        }

        private void OnDisable()
        {
            UnbindInput();
        }

        private void HandleTutorialEnemySpawn(TutorialEnemySpawnEvent obj)
        {
            BindTutorialEnemy(obj.Enemy);
            _showAttackGuideAfterEnemySpawn = true;

            if (!_hasPlayedIntro)
            {
                _hasPlayedIntro = true;
                PlayQueuedTutorial(
                    new TutorialLine(catIntroText1, catSprite),
                    new TutorialLine(catIntroText2, catSprite),
                    new TutorialLine(catIntroText3, catSprite),
                    new TutorialLine(lukaIntroText, lukaSprite),
                    new TutorialLine(enemySpawnText, lukaSprite));
                return;
            }

            TutorialGo(enemySpawnText, lukaSprite);
        }

        private void HandleTutorialBossSpawn(TutorialBossSpawnEvent obj)
        {
            TutorialGo(bossSpawnText, lukaSprite);
        }

        private void HandleTutorialEnemyDeath()
        {
            UnbindTutorialEnemy();
            TutorialGo(enemyDeathText, lukaSprite);
        }

        public void TutorialGo(string text, Sprite sprite)
        {
            _lineQueue.Clear();
            UnbindInput();
            ShowTutorial(text, sprite);
            BindInput();
        }

        public void TryCompleteTutorial()
        {
            if (Time.unscaledTime < _canCompleteTime)
                return;

            CompleteTutorial();
        }

        public void CompleteTutorial()
        {
            if (!_isPlaying)
                return;

            if (TryShowNextQueuedLine())
                return;

            if (_showAttackGuideAfterEnemySpawn)
            {
                _showAttackGuideAfterEnemySpawn = false;
                TutorialGo(attackGuideText, lukaSprite);
                return;
            }

            _isPlaying = false;
            UnbindInput();
            Time.timeScale = 1f;

            if (guideRoot != null)
                guideRoot.SetActive(false);

            OnTutorialCompleted?.Invoke();
        }

        private void PlayQueuedTutorial(params TutorialLine[] lines)
        {
            _lineQueue.Clear();

            if (lines == null || lines.Length <= 0)
                return;

            for (int i = 1; i < lines.Length; i++)
                _lineQueue.Enqueue(lines[i]);

            TutorialLine firstLine = lines[0];
            UnbindInput();
            ShowTutorial(firstLine.Text, firstLine.Sprite);
            BindInput();
        }

        private bool TryShowNextQueuedLine()
        {
            if (_lineQueue.Count <= 0)
                return false;

            TutorialLine nextLine = _lineQueue.Dequeue();
            UnbindInput();
            ShowTutorial(nextLine.Text, nextLine.Sprite);
            BindInput();
            return true;
        }

        private void ShowTutorial(string text, Sprite sprite)
        {
            _isPlaying = true;

            if (guideRoot != null)
                guideRoot.SetActive(true);

            if (characterImage != null)
                characterImage.sprite = sprite;

            if (tutorialText != null)
                tutorialText.text = text;

            _canCompleteTime = Time.unscaledTime + clickDelay;
            Time.timeScale = 0f;
        }

        private void BindTutorialEnemy(AbstractEnemy enemy)
        {
            UnbindTutorialEnemy();

            _tutorialEnemy = enemy;
            if (_tutorialEnemy == null || _tutorialEnemy.HealthModule == null)
                return;

            _tutorialEnemy.HealthModule.OnDeath -= HandleTutorialEnemyDeath;
            _tutorialEnemy.HealthModule.OnDeath += HandleTutorialEnemyDeath;
        }

        private void UnbindTutorialEnemy()
        {
            if (_tutorialEnemy != null && _tutorialEnemy.HealthModule != null)
                _tutorialEnemy.HealthModule.OnDeath -= HandleTutorialEnemyDeath;

            _tutorialEnemy = null;
        }

        private void BindInput()
        {
            if (playerInput == null)
                return;

            playerInput.OnLeftMousePressedStart += TryCompleteTutorial;
        }

        private void UnbindInput()
        {
            if (playerInput == null)
                return;

            playerInput.OnLeftMousePressedStart -= TryCompleteTutorial;
        }

        private void HideImmediate()
        {
            if (guideRoot != null)
                guideRoot.SetActive(false);
        }

        private readonly struct TutorialLine
        {
            public readonly string Text;
            public readonly Sprite Sprite;

            public TutorialLine(string text, Sprite sprite)
            {
                Text = text;
                Sprite = sprite;
            }
        }
    }
}
