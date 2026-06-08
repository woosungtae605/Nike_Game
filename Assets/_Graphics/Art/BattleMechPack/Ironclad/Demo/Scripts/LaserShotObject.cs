// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using UnityEngine;

namespace PampelArt.Characters.Ironclad.Demo
{
    public class LaserShotObject : MonoBehaviour
    {
        public GameObject explosion;
        public float speed = 1f;
        
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
            transform.Translate(Vector3.forward * (Time.deltaTime * speed), Space.Self);
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
