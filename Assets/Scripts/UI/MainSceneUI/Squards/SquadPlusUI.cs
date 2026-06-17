using System.Collections.Generic;
using Agents.Players;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainSceneUI.Squards
{
    public class SquadPlusUI : MonoBehaviour
    {
        [SerializeField] private PlayerSquadSO playerSquad;
        [SerializeField] private Image[] images = new Image[5];
        [SerializeField] private Sprite noEquip;
        public void Init()
        {
            for (int i = 0; i < playerSquad.PlayerDataSos.Length; i++)
            {
                images[i].sprite =  playerSquad.PlayerDataSos[i] != null ? playerSquad.PlayerDataSos[i].NikkeSprite : noEquip;
            }
        }
    }
}