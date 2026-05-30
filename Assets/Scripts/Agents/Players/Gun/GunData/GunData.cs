using System;
using UnityEngine;

namespace Agents.Players.Gun.GunData
{
    public enum CastType
    {
        Ray, Sphere, Box
    }
    [Serializable]
    public abstract class GunData
    {
        [Header("Common")]
        [SerializeField] private int damage = 10;
        [SerializeField] private float fireInterval = 0.1f;
        [SerializeField] private int maxAmmo = 30;
        [SerializeField] private float reloadTime = 1.5f;
        [SerializeField] private float maxDistance = 20f;
        [SerializeField] private LayerMask hitMask;
        [SerializeField] private CastType castType;
        
        [Header("Camera")]
        [SerializeField] private float cameraShakePower = 0.1f;
        [SerializeField] private float cameraShakeDuration = 0.08f;
        
        public int Damage => damage;
        public float FireInterval => fireInterval;
        public int MaxAmmo => maxAmmo;
        public float ReloadTime => reloadTime;
        public float MaxDistance => maxDistance;
        public LayerMask HitMask => hitMask;
        public float CameraShakePower => cameraShakePower;
        public float CameraShakeDuration => cameraShakeDuration;
        public CastType CastType => castType;

        public abstract void Shot(PlayerGun playerGunOwner);
    }
}