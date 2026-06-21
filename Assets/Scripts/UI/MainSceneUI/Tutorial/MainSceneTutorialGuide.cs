using System;
using System.Collections;
using Agents.Players;
using Systems.SaveSystem;
using TMPro;
using UI.MainSceneUI.Bottoms;
using UI.MainSceneUI.Nikkes;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace UI.MainSceneUI.Tutorial
{
    public class MainSceneTutorialGuide : MonoBehaviour
    {
        [Header("Targets")]
        [SerializeField] private BottomButtonController bottomButtonController;
        [SerializeField] private NikkeContainer nikkeContainer;
        [SerializeField] private RectTransform nikkeButton;
        [SerializeField] private TutorialBlockerImage blockerImage;
        [SerializeField] private GameObject guideRoot;
        [SerializeField] private TextMeshProUGUI tutorialText;

        [Header("Save")]
        [SerializeField] private SaveFileNameSO saveFileName;

        [Header("Nikke Tutorial")]
        [SerializeField] private ButtonsType targetButtonType = ButtonsType.Nikke;
        [SerializeField] private string guideText = "한번 냥케로 들어가봐.";
        [SerializeField] private string clickProfileText = "클릭해.";
        [SerializeField] private string upgradeGuideText = "방금 얻은 돈으로 이제 냥케를 업그레이드 할 수 있어.";

        [Header("Squad Tutorial")]
        [SerializeField] private ButtonsType squadButtonType = ButtonsType.Squard;
        [SerializeField] private string squadGuideText = "여기선 캐릭터를 장착시킬 수 있어. 다시 뺄려면 장착되있는 칸을 눌러서 빼면 돼.";

        [Header("Settings")]
        [SerializeField] private float profileFocusDelay = 0.12f;
        [SerializeField] private float messageClickDelay = 0.15f;
        [SerializeField] private bool showOnStart = true;

        private TutorialStep _step;
        private Coroutine _focusProfileRoutine;
        private float _messageCanCloseTime;

        private enum TutorialStep
        {
            None,
            OpenNikke,
            ClickFirstProfile,
            UpgradeGuideMessage,
            SquadGuideMessage
        }

        private void Awake()
        {
            if (bottomButtonController != null)
                bottomButtonController.OnClickButton += HandleBottomButtonClick;

            if (nikkeContainer != null)
                nikkeContainer.OnClickProfile += HandleClickProfile;
        }

        private void Start()
        {
            if (IsNikkeCleared())
            {
                Hide();
                return;
            }

            if (showOnStart)
                Show();
        }

        private void Update()
        {
            if (_step != TutorialStep.UpgradeGuideMessage && _step != TutorialStep.SquadGuideMessage)
                return;

            if (Time.unscaledTime < _messageCanCloseTime)
                return;

            if (Mouse.current == null || !Mouse.current.leftButton.wasPressedThisFrame)
                return;

            if (_step == TutorialStep.SquadGuideMessage)
                MarkSquadCleared();
            else
                MarkNikkeCleared();

            Hide();
        }

        private void OnDestroy()
        {
            if (bottomButtonController != null)
                bottomButtonController.OnClickButton -= HandleBottomButtonClick;

            if (nikkeContainer != null)
                nikkeContainer.OnClickProfile -= HandleClickProfile;

            StopFocusProfileRoutine();
        }

        public void Show()
        {
            if (IsNikkeCleared())
                return;

            _step = TutorialStep.OpenNikke;

            if (nikkeButton == null && bottomButtonController != null)
                bottomButtonController.TryGetButtonRect(targetButtonType, out nikkeButton);

            SetGuideText(guideText);
            SetBlockTarget(nikkeButton);
            SetGuideActive(true);
        }

        public void Hide()
        {
            _step = TutorialStep.None;
            StopFocusProfileRoutine();

            if (blockerImage != null)
                blockerImage.gameObject.SetActive(false);

            SetGuideActive(false);
        }

        private void HandleBottomButtonClick(ButtonsType buttonType)
        {
            if (_step == TutorialStep.OpenNikke && buttonType == targetButtonType)
            {
                StopFocusProfileRoutine();
                _focusProfileRoutine = StartCoroutine(FocusFirstProfileRoutine());
                return;
            }

            if (_step != TutorialStep.None || buttonType != squadButtonType || IsSquadCleared())
                return;

            ShowSquadGuideMessage();
        }

        private IEnumerator FocusFirstProfileRoutine()
        {
            if (profileFocusDelay > 0f)
                yield return new WaitForSeconds(profileFocusDelay);
            else
                yield return null;

            Canvas.ForceUpdateCanvases();

            if (nikkeContainer != null && nikkeContainer.TryGetFirstProfileRect(out RectTransform profileRect))
            {
                _step = TutorialStep.ClickFirstProfile;
                SetGuideText(clickProfileText);
                SetBlockTarget(profileRect);
            }

            _focusProfileRoutine = null;
        }

        private void HandleClickProfile(PlayerDataSO playerData)
        {
            if (_step != TutorialStep.ClickFirstProfile)
                return;

            ShowUpgradeGuideMessage();
        }

        private void ShowUpgradeGuideMessage()
        {
            _step = TutorialStep.UpgradeGuideMessage;
            _messageCanCloseTime = Time.unscaledTime + messageClickDelay;

            SetGuideText(upgradeGuideText);
            SetBlockTarget(null);
            SetGuideActive(true);
        }

        private void ShowSquadGuideMessage()
        {
            _step = TutorialStep.SquadGuideMessage;
            _messageCanCloseTime = Time.unscaledTime + messageClickDelay;

            SetGuideText(squadGuideText);
            SetBlockTarget(null);
            SetGuideActive(true);
        }

        private bool IsNikkeCleared()
        {
            return LoadSaveData().isCleared;
        }

        private bool IsSquadCleared()
        {
            return LoadSaveData().isSquadCleared;
        }

        private MainSceneTutorialSaveData LoadSaveData()
        {
            if (!JsonSaveService.TryLoad(saveFileName, out MainSceneTutorialSaveData saveData) || saveData == null)
                return new MainSceneTutorialSaveData();

            return saveData;
        }

        private void MarkNikkeCleared()
        {
            MainSceneTutorialSaveData saveData = LoadSaveData();
            saveData.isCleared = true;
            JsonSaveService.Save(saveFileName, saveData);
        }

        private void MarkSquadCleared()
        {
            MainSceneTutorialSaveData saveData = LoadSaveData();
            saveData.isSquadCleared = true;
            JsonSaveService.Save(saveFileName, saveData);
        }

        [ContextMenu("Reset Tutorial Save")]
        private void ResetTutorialSave()
        {
            JsonSaveService.Delete(saveFileName);
        }

        private void SetGuideText(string text)
        {
            if (tutorialText != null)
                tutorialText.text = text;
        }

        private void SetBlockTarget(RectTransform target)
        {
            if (blockerImage == null)
                return;

            blockerImage.gameObject.SetActive(true);
            blockerImage.SetPassThroughTarget(target);
            blockerImage.raycastTarget = true;
        }

        private void SetGuideActive(bool active)
        {
            if (guideRoot != null)
                guideRoot.SetActive(active);
            else
                gameObject.SetActive(active);
        }

        private void StopFocusProfileRoutine()
        {
            if (_focusProfileRoutine == null)
                return;

            StopCoroutine(_focusProfileRoutine);
            _focusProfileRoutine = null;
        }

        [Serializable]
        private class MainSceneTutorialSaveData
        {
            public bool isCleared;
            public bool isSquadCleared;
        }
    }
}
