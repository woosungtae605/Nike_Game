using Agents.Enemies;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.BattleUI.WaveUI
{
    public class WaveBossHealthCanvas : MonoBehaviour
    {
        [Header("Uis")] 
        public Slider enemyHealthSlider;
        
        private AbstractEnemy _enemy;

        public void SetEnemy(AbstractEnemy enemy)
        {
            _enemy = enemy;
        }
    }
}