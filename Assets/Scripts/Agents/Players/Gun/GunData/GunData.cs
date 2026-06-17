using System;
using Agents.Enemies;
using UnityEngine;

namespace Agents.Players.Gun.GunData
{
    [Serializable]
    public abstract class GunData
    {
        [field : SerializeField] public string GunName { get; private set; }
        
        [Header("Common")]
        [SerializeField] private int damage = 10;
        [SerializeField] private float fireInterval = 0.1f;
        [SerializeField] private int maxAmmo = 30;
        [SerializeField] private float reloadTime = 1.5f;
        [SerializeField] private float maxDistance = 20f;
        [SerializeField] private float aiRandomSpreadAngle = 1f;
        [SerializeField] private LayerMask hitMask;
        
        [Header("Camera")]
        [SerializeField] private float cameraShakePower = 0.1f;
        [SerializeField] private float cameraShakeDuration = 0.08f;
        
        [Header("Effects")]
        [SerializeField] private float lineEffectDuration = 0.05f;
        
        public int Damage => damage;
        public float FireInterval => fireInterval;
        public int MaxAmmo => maxAmmo;
        public float ReloadTime => reloadTime;
        public float MaxDistance => maxDistance;
        public float AIRandomSpreadAngle => aiRandomSpreadAngle;
        public LayerMask HitMask => hitMask;
        public float CameraShakePower => cameraShakePower;
        public float CameraShakeDuration => cameraShakeDuration;
        public float LineEffectDuration => lineEffectDuration;

        public abstract bool Shot(PlayerGun playerGunOwner);        
        public abstract bool ShotAI(PlayerGun playerGunOwner, AbstractEnemy target);

        public abstract AbstractEnemy SelectAITarget(EnemyRegisterSo enemyRegisterSo, Transform myTransform);
    

        protected Vector3 GetSpreadDirection(Vector3 baseDirection, float angle)
        {
            float halfAngle = angle * 0.5f;
            Vector3 randomOffset = new Vector3(UnityEngine.Random.Range(-halfAngle, halfAngle), UnityEngine.Random.Range(-halfAngle, halfAngle), 0f);
            return Quaternion.Euler(randomOffset) * baseDirection;
        }
}
}