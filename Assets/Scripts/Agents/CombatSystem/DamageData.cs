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

        public DamageData(Agent attacker, int damage,  Vector3 hitPoint, Vector3 hitNormal, float hitDistance)
        {
            Attacker = attacker;
            Damage = damage;
            HitPoint = hitPoint;
            HitNormal = hitNormal;
            HitDistance = hitDistance;
        }
    }
}