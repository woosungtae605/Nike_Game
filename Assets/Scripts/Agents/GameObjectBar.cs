using Agents.Module;
using Module;
using UnityEngine;

namespace Agents
{
    public class GameObjectBar : MonoBehaviour
    {
        [SerializeField] private GameObject fill;
        
        private Agent _agent;
        private IBar _bar;
        private Vector3 _originFillScale;

        public void Initialize(ModuleOwner owner)
        {
            _agent = owner as Agent;
        }
        
        private void OnDestroy()
        {
            if (_bar != null)
                _bar.OnChanged -= HandleBarChanged;
        }

        private void HandleBarChanged(int currentHealth, int maxHealth)
        {
            if (fill == null || maxHealth <= 0)
                return;
            
            float ratio = (float)currentHealth / maxHealth;
            ratio = Mathf.Clamp01(ratio);

            fill.transform.localScale = new Vector3(_originFillScale.x * ratio, _originFillScale.y, _originFillScale.z);
        }
    }
}