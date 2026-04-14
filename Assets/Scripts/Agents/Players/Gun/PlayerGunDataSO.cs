using System;
using Agents.Players.Gun.GunData;
using UnityEngine;

namespace Agents.Players.Gun
{
    [CreateAssetMenu(fileName = "Player gun data", menuName = "SO/gunData", order = 0)]
    public class PlayerGunDataSO : ScriptableObject
    {
        [SerializeReference] private GunData.GunData gunData;
        public GunData.GunData GunData => gunData;
        public string className;
    }
}