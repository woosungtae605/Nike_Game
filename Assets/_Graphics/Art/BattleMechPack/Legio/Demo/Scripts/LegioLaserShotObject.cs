// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using UnityEngine;

namespace Art.Characters.Legio.Demo
{
    public class LegioLaserShotObject : MonoBehaviour
    {
        public GameObject explosion;
        public float speed = 15f;
        public float acceleration = 0.25f; 
        public float accelerationRate = 2f;

        private ParticleSystem particles;
        private float maxTime;
        
        private void Awake()
        {
            particles = GetComponent<ParticleSystem>();
            particles.Play();
            maxTime = Time.unscaledTime;
        }


        private void Update()
        {
            acceleration += Time.deltaTime * accelerationRate; 
            acceleration = Mathf.Clamp01(acceleration); 

            float currentSpeed = speed * acceleration;
            transform.Translate(Vector3.forward * (Time.deltaTime * currentSpeed), Space.Self);

            if (Time.unscaledTime > maxTime + 5f)
            {
                Destroy(gameObject);
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
