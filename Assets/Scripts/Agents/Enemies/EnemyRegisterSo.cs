using System;
using System.Collections.Generic;
using UnityEngine;

namespace Agents.Enemies
{
    [CreateAssetMenu(fileName = "EnemyRegister", menuName = "SO/Enemy/Register", order = 0)]
    public class EnemyRegisterSo : ScriptableObject
    {
        private readonly List<AbstractEnemy> _enemies = new();
        
        public int EnemyCount => _enemies.Count;

        public void Clear()
        {
            _enemies.Clear();
        }

        public void Register(AbstractEnemy abstractEnemy)
        {
            if(!_enemies.Contains(abstractEnemy))
                _enemies.Add(abstractEnemy);
        }

        public void UnRegister(AbstractEnemy abstractEnemy)
        {
            _enemies.Remove(abstractEnemy);
        }

        public AbstractEnemy ClosestEnemy(Transform myPos)
        {
            AbstractEnemy closestAbstractEnemy = null;
            float closestDistance = float.MaxValue;

            for (int i = _enemies.Count - 1; i >= 0; i--)
            {
                AbstractEnemy abstractEnemy = _enemies[i];

                if (abstractEnemy == null)
                {
                    _enemies.RemoveAt(i);
                    continue;
                }

                float distance = (abstractEnemy.transform.position - myPos.position).sqrMagnitude;

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestAbstractEnemy = abstractEnemy;
                }
            }

            return closestAbstractEnemy;
        }

        public AbstractEnemy FurthestEnemy(Transform myPos)
        {
            AbstractEnemy furthestAbstractEnemy = null;
            float furthestDistance = float.MinValue;

            for (int i = _enemies.Count - 1; i >= 0; i--)
            {
                AbstractEnemy abstractEnemy = _enemies[i];

                if (abstractEnemy == null)
                {
                    _enemies.RemoveAt(i);
                    continue;
                }

                float distance = (abstractEnemy.transform.position - myPos.position).sqrMagnitude;

                if (distance > furthestDistance)
                {
                    furthestDistance = distance;
                    furthestAbstractEnemy = abstractEnemy;
                }
            }

            return furthestAbstractEnemy;
        }
    }
}