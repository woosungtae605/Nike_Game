using UnityEngine;

namespace Agents.Missiles
{
    [CreateAssetMenu(fileName = "Missile", menuName = "SO/Missile", order = 0)]
    public class MissileSO : ScriptableObject
    {
        [field: SerializeField] public int MaxHealth { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }
    }
}