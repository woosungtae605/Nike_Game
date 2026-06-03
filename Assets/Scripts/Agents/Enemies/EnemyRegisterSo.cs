using System;
using System.Collections.Generic;
using UnityEngine;

namespace Agents.Enemies
{
    [CreateAssetMenu(fileName = "EnemyRegister", menuName = "SO/Enemy/Register", order = 0)]
    public class EnemyRegisterSo : ScriptableObject
    {
        private readonly List<Enemy> _enemies = new();


        public void Clear()
        {
            _enemies.Clear();
        }

        public void Register(Enemy enemy)
        {
            if(!_enemies.Contains(enemy))
                _enemies.Add(enemy);
        }

        public void UnRegister(Enemy enemy)
        {
            _enemies.Remove(enemy);
        }

        public Enemy ClosestEnemy(Transform myPos)
        {
            Enemy closestEnemy = null;
            float closestDistance = float.MaxValue;

            for (int i = _enemies.Count - 1; i >= 0; i--)
            {
                Enemy enemy = _enemies[i];

                if (enemy == null)
                {
                    _enemies.RemoveAt(i);
                    continue;
                }

                float distance = (enemy.transform.position - myPos.position).sqrMagnitude;

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }

            return closestEnemy;
        }

        public Enemy FurthestEnemy(Transform myPos)
        {
            Enemy furthestEnemy = null;
            float furthestDistance = float.MinValue;

            for (int i = _enemies.Count - 1; i >= 0; i--)
            {
                Enemy enemy = _enemies[i];

                if (enemy == null)
                {
                    _enemies.RemoveAt(i);
                    continue;
                }

                float distance = (enemy.transform.position - myPos.position).sqrMagnitude;

                if (distance > furthestDistance)
                {
                    furthestDistance = distance;
                    furthestEnemy = enemy;
                }
            }

            return furthestEnemy;
        }
    }
}