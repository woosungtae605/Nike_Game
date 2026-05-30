using UnityEngine;

namespace Agents.CombatSystem
{
    public struct DamageData
    {
        public Agent Attacker;
        public int Damage;
        public bool Weakness;
        public Vector3 HitPoint;
        public Vector3 HitNormal;

        public DamageData(Agent attacker, int damage, bool weakness,  Vector3 hitPoint, Vector3 hitNormal)
        {
            Attacker = attacker;
            Damage = damage;
            Weakness = weakness;
            HitPoint = hitPoint;
            HitNormal = hitNormal;
        }
    }
}