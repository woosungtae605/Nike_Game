using CoreSystem.BusSystem;
using GameEvents.UI;
using UnityEngine;

namespace Agents.CombatSystem
{
    public class DamageHitBox : MonoBehaviour, IDamageable
    {
        private Agent owner;
        [SerializeField] private float damageMultiplier = 1.5f;
        [SerializeField] private bool isCritical;

        private void Awake()
        {
            if (owner == null)
                owner = GetComponentInParent<Agent>();
        }

        public void ApplyDamage(DamageData damageData)
        {
            if (owner == null)
                return;

            int finalDamage = Mathf.RoundToInt(damageData.Damage * damageMultiplier);
            damageData.Damage = finalDamage;

            owner.ApplyDamage(damageData);
            Bus<HitCursorUIEvent>.Raise(new HitCursorUIEvent(isCritical));  
        }
    }
}
