using UnityEngine;

namespace Agents.Enemies
{
    public class EnemyGotoArea : MonoBehaviour
    {
        [SerializeField] private bool gotoLeft = true;

        private void OnTriggerEnter(Collider other)
        {
            SetDirection(other);
        }

        private void SetDirection(Collider other)
        {
            AbstractEnemy enemy = other.GetComponentInParent<AbstractEnemy>();
            if (enemy != null)
                enemy.SetGotoLeft(gotoLeft);
        }

        private void OnValidate()
        {
            if (TryGetComponent(out Collider triggerCollider))
                triggerCollider.isTrigger = true;
        }
    }
}
