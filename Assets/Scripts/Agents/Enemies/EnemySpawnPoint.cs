using System;
using Reflex.Attributes;
using Reflex.Core;
using UnityEngine;

namespace Agents.Enemies
{
    public class EnemySpawnPoint : MonoBehaviour, IInstaller
    {
        [SerializeField] private Transform[] spawnPoints;
        
        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterValue(spawnPoints);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                Gizmos.DrawSphere(spawnPoints[i].position, 1);
            }
        }
    }
}