using UnityEngine;

namespace Agents.CombatSystem
{
    [CreateAssetMenu(fileName = "Skill data", menuName = "Skill data", order = 0)]
    public class SkillDataSO : ScriptableObject
    {
        public int skillIndex;
        public string skillName;
        public float cooldown;
        public int damage = 10;
        public float maxDistance = 60f;
        public LayerMask hitMask = 1 << 7;
    }
}
