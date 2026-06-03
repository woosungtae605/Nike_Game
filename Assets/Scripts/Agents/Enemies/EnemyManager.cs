using Gamelib.ObjectPool.Runtime;
using UnityEngine;

namespace Agents.Enemies
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private PoolManagerSo poolManagerSo;
        [SerializeField] private EnemyRegisterSo enemyRegisterSo;

        public EnemyRegisterSo EnemyRegister => enemyRegisterSo;
    }
}