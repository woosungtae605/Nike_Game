using Agents.Players.Gun;
using UnityEngine;
using UnityEngine.UI;

namespace Agents.Players
{
    [CreateAssetMenu(fileName = "Player data", menuName = "SO/playerData", order = 0)]
    public class PlayerDataSO : ScriptableObject
    {
        [field: SerializeField] public PlayerGunDataSO PlayerGunData { get; private set; }
        [field: SerializeField] public Sprite NikkeSprite { get; private set; }
        [field: SerializeField] public int NikkeHp { get; private set; }
        [field: SerializeField] public int NikkeDamage { get; private set; }
    }
}