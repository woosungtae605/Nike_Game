using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainSceneUI.Nikkes.NikkeProfiles
{
    public class NikkeProfile : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Image profileImage;
        [SerializeField] private Button clickBtn;
    }
}