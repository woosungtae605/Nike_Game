using System.Collections.Generic;
using System.Collections;
using System;
using Agents.Players;
using Systems.UpgradeSystem;
using UI.MainSceneUI.Nikkes.NikkeProfiles;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainSceneUI.Nikkes
{
    public class NikkeContainer : MonoBehaviour
    {
        [SerializeField] private NikkeProfile profilePrefab;
        [SerializeField] private Transform profileParent;
        [SerializeField] private int defaultPoolCount = 10;
        [SerializeField] private float showInterval = 0.05f;
        [SerializeField] private UpgradeManager upgradeManager;

        private readonly Queue<NikkeProfile> _profilePool = new Queue<NikkeProfile>();
        private readonly List<NikkeProfile> _activeProfiles = new List<NikkeProfile>();
        private Coroutine _showRoutine;

        public event Action<PlayerDataSO> OnClickProfile;

        private void Awake()
        {
            if (profileParent == null)
                profileParent = transform;

            if (upgradeManager == null)
                upgradeManager = FindFirstObjectByType<UpgradeManager>();

            CreatePool(defaultPoolCount);
        }

        private void OnDestroy()
        {
            StopShowRoutine();

            foreach (NikkeProfile profile in _activeProfiles)
            {
                if (profile != null)
                    profile.OnClickBtn -= HandleClickProfile;
            }

            foreach (NikkeProfile profile in _profilePool)
            {
                if (profile != null)
                    profile.OnClickBtn -= HandleClickProfile;
            }
        }

        public void Init(PlayerDataSos playerDataSos)
        {
            StopShowRoutine();
            HideAllProfiles();

            if (playerDataSos == null)
                return;

            foreach (PlayerDataSO playerData in playerDataSos.AllPlayerDatas)
            {
                if (playerData == null)
                    continue;

                NikkeProfile profile = PopProfile();
                if (profile == null)
                    return;

                profile.Show(playerData, GetDisplayLevel(playerData));
                _activeProfiles.Add(profile);
            }

            RebuildLayout();

            _showRoutine = StartCoroutine(ShowProfilesRoutine());
        }

        private IEnumerator ShowProfilesRoutine()
        {
            foreach (NikkeProfile profile in _activeProfiles)
            {
                if (profile != null)
                    profile.PlayShowAnimation();

                if (showInterval > 0f)
                    yield return new WaitForSeconds(showInterval);
            }

            _showRoutine = null;
        }

        private void CreatePool(int count)
        {
            if (profilePrefab == null)
                return;

            for (int i = 0; i < count; i++)
            {
                NikkeProfile profile = Instantiate(profilePrefab, profileParent);
                profile.OnClickBtn -= HandleClickProfile;
                profile.OnClickBtn += HandleClickProfile;
                profile.Hide();
                _profilePool.Enqueue(profile);
            }
        }

        private NikkeProfile PopProfile()
        {
            if (_profilePool.Count <= 0)
                CreatePool(1);

            if (_profilePool.Count <= 0)
                return null;

            return _profilePool.Dequeue();
        }

        private void HideAllProfiles()
        {
            for (int i = 0; i < _activeProfiles.Count; i++)
            {
                NikkeProfile profile = _activeProfiles[i];
                if (profile == null)
                    continue;

                profile.Hide();
                _profilePool.Enqueue(profile);
            }

            _activeProfiles.Clear();
        }

        private void StopShowRoutine()
        {
            if (_showRoutine == null)
                return;

            StopCoroutine(_showRoutine);
            _showRoutine = null;
        }

        private void RebuildLayout()
        {
            Canvas.ForceUpdateCanvases();

            if (profileParent is RectTransform parentRect)
                LayoutRebuilder.ForceRebuildLayoutImmediate(parentRect);

            Canvas.ForceUpdateCanvases();
        }

        private void HandleClickProfile(PlayerDataSO playerData)
        {
            if (playerData == null)
                return;

            OnClickProfile?.Invoke(playerData);
        }

        public bool TryGetFirstProfileRect(out RectTransform profileRect)
        {
            profileRect = null;

            if (_activeProfiles.Count <= 0 || _activeProfiles[0] == null)
                return false;

            profileRect = _activeProfiles[0].transform as RectTransform;
            return profileRect != null;
        }
        public void RefreshProfileLevels()
        {
            foreach (NikkeProfile profile in _activeProfiles)
            {
                if (profile == null)
                    continue;

                PlayerDataSO playerData = profile.PlayerData;
                if (playerData == null)
                    continue;

                profile.SetLevel(GetDisplayLevel(playerData));
            }
        }

        private int GetDisplayLevel(PlayerDataSO playerData)
        {
            if (upgradeManager == null)
                return 1;

            return upgradeManager.GetLevel(playerData) + 1;
        }
    }
}

