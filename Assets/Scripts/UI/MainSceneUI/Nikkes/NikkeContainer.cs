using System.Collections.Generic;
using System.Collections;
using Agents.Players;
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

        private readonly Queue<NikkeProfile> _profilePool = new Queue<NikkeProfile>();
        private readonly List<NikkeProfile> _activeProfiles = new List<NikkeProfile>();
        private Coroutine _showRoutine;

        private void Awake()
        {
            if (profileParent == null)
                profileParent = transform;

            CreatePool(defaultPoolCount);
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

                profile.Show(playerData);
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
    }
}
