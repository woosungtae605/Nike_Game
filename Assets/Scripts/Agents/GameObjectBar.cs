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
        
        private void Awake()
        {
            if (fill != null)
                _originFillScale = fill.transform.localScale;
        }

        public void SetValue(int currentValue, int maxValue)
        {
            if (fill == null || maxValue <= 0)
                return;

            float ratio = Mathf.Clamp01((float)currentValue / maxValue);

            fill.transform.localScale = new Vector3(
                _originFillScale.x * ratio,
                _originFillScale.y,
                _originFillScale.z
            );
        }
    }
}