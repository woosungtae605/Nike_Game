using System;
using UnityEngine;

namespace Agents.Players.Gun.GunData
{
    [Serializable]
    public abstract class GunData
    {
        [Header("Common")]
        [SerializeField] private float damage = 10f;
        [SerializeField] private float fireInterval = 0.1f;
        [SerializeField] private int maxAmmo = 30;
        [SerializeField] private float reloadTime = 1.5f;
        [SerializeField] private float maxDistance = 20f;
        [SerializeField] private LayerMask hitMask;
        
        [Header("Camera")]
        [SerializeField] private float cameraShakePower = 0.1f;
        [SerializeField] private float cameraShakeDuration = 0.08f;
        
        public float Damage => damage;
        public float FireInterval => fireInterval;
        public int MaxAmmo => maxAmmo;
        public float ReloadTime => reloadTime;
        public float MaxDistance => maxDistance;
        public LayerMask HitMask => hitMask;
        public float CameraShakePower => cameraShakePower;
        public float CameraShakeDuration => cameraShakeDuration;

        public abstract void Shot(Gun gunOwner);
    }
}