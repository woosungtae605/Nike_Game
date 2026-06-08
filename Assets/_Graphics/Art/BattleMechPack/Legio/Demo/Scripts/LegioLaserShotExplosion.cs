// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using UnityEngine;

namespace Art.Characters.Legio.Demo
{
    public class LegioLaserShotExplosion : MonoBehaviour
    {
        public AudioSource audioSource;
        private ParticleSystem explosion;
        

        private void Awake()
        {
            if(audioSource != null)
                audioSource.Play();
            explosion = GetComponent<ParticleSystem>();
            var explosionMain = explosion.main;
            explosionMain.stopAction = ParticleSystemStopAction.Callback;
            explosion.Play();
        }
    
    
        void OnParticleSystemStopped()
        {
            Destroy(gameObject);
        }
    
    
    
    }
}
