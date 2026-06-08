// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace PampelArt.Characters.ShockBomber.Demo
{
    
    public class ShockBomberRocket : MonoBehaviour
    {
        private void Reset()
        {
            collisionLayers |= 1 << LayerMask.NameToLayer("Default");
        }

        public Transform startPosition;
        public ParticleSystem explosion;
        public ParticleSystem trail;

        public LayerMask collisionLayers;
        public UnityEvent onLaunch;
        public UnityEvent onCollision;

        public float delay = 0f;
        public float acceleration = 1f; // The rate at which the rocket's speed increases
        public float maxSpeed = 50f; // The maximum speed the rocket can reach
        private Vector3 velocity; // The current velocity of the rocket
        public float maxTime = 10f; // The maximum time the rocket can fly before stopping


        


        public void LaunchRocket()
        {
            gameObject.SetActive(true);            
            StartCoroutine(_FlyRocket());
        }


        private IEnumerator _FlyRocket()
        {
            float elapsedTime = 0f; // The elapsed time since the coroutine started

            yield return new WaitForSeconds(delay);
            
            trail.Play();
            onLaunch.Invoke();
            
            while (true & elapsedTime < maxTime)
            {
                var right = transform.forward;
                velocity += right * (acceleration * Time.fixedDeltaTime);
                velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

                // Move the rocket in the local forward direction using the new velocity
                transform.position += right * (velocity.magnitude * Time.fixedDeltaTime);

                elapsedTime += Time.deltaTime;

                yield return null;    
            }
            ActivateImpact();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (collisionLayers != (collisionLayers | (1 << other.gameObject.layer))) return; 
            StopCoroutine(_FlyRocket());
            ActivateImpact();
        }


        private void ActivateImpact()
        {
            onCollision.Invoke();
            explosion.transform.position = transform.position;
            explosion.Play();
            ResetRocket();
        }
        
        private void ResetRocket()
        {
            transform.position = startPosition.transform.position;
            transform.rotation = startPosition.transform.rotation;

            trail.Stop();
            velocity = Vector3.zero;
            gameObject.SetActive(false);
        }
    }
}
