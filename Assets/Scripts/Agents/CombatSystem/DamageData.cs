using UnityEngine;

namespace Agents.CombatSystem
{
    public struct DamageData
    {
        public Agent Attacker;
        public int Damage;
        public Vector3 HitPoint;
        public Vector3 HitNormal;
        public float HitDistance;
        public bool IsCritical;
        public float DamageMultiplier;


        public DamageData(Agent attacker, int damage,  Vector3 hitPoint, Vector3 hitNormal, float hitDistance)
        {
            Attacker = attacker;
            Damage = damage;
            HitPoint = hitPoint;
            HitNormal = hitNormal;
            HitDistance = hitDistance;
            IsCritical = false;
            DamageMultiplier = 1f;

        }
    }
}