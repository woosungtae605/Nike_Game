using Agents.Players.Gun;
using UnityEngine;
using UnityEngine.UI;

namespace Agents.Players
{
    [CreateAssetMenu(fileName = "Player data", menuName = "SO/playerData", order = 0)]
    public class PlayerDataSO : ScriptableObject
    {
        [field: SerializeField] public int NikkeID { get; private set; }
        [field: SerializeField] public string NikkeName { get; private set; }
        [field: SerializeField] public int Age { get; private set; }
        [field: SerializeField, TextArea(3, 10)] public string NikkeDescription { get; private set; }
        [field: SerializeField] public string Sexuality { get; private set; }
        [field: SerializeField] public PlayerGunDataSO PlayerGunData { get; private set; }
        [field: SerializeField] public Sprite NikkeSprite { get; private set; }
        [field: SerializeField] public int MaxHp { get; private set; }
        [field: SerializeField] public Player player;
    }
}