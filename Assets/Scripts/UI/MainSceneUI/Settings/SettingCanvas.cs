using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainSceneUI.Settings
{
    public class SettingCanvas : MonoBehaviour
    {
        [SerializeField] private Button settingBtn;
        [SerializeField] private GameObject openGameObject;

        private void Awake()
        {
            settingBtn.onClick.AddListener(HandleClickSettingBtn);
        }

        private void OnDestroy()
        {
            settingBtn.onClick.RemoveListener(HandleClickSettingBtn);
        }

        private void HandleClickSettingBtn()
        {
            openGameObject.SetActive(true);
        }
    }
}