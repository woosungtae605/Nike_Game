using Gamelib.ObjectPool.Runtime;
using UnityEngine;

namespace Agents.Enemies
{
    public class Enemy : Agent, IPoolable
    {
        #region Pool
        public PoolItemSo PoolItem { get; set; }
        public GameObject GameObject => this != null ? gameObject : null;
        public void ResetItem()
        {
            
        }
        #endregion
        
        
    }
}