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

        public void SetFirstRun(bool value)
        {
            FirstRun = value;
        }

        private void Start()
        {
            NavMovement.Speed = EnemyDataSo.Speed;
            NavMovement.NavAgent.updateRotation = false;
        }
    }
}