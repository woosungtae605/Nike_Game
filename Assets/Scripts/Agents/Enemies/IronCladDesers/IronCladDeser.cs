using System;
using UnityEngine;

namespace Agents.Enemies.IronCladDesers
{
    public class IronCladDeser : AbstractEnemy
    {
        public bool FirstRun { get; private set; } = false;

        public override void ResetItem()
        {
            base.ResetItem();
            FirstRun = true;
        }

        private void Start()
        {
            NavMovement.Speed = EnemyDataSo.Speed;
        }
    }
}