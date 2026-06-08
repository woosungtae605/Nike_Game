// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Art.Characters.Legio.Demo
{
    public class LegioMachineGun : MonoBehaviour
    {

        public string animatorDeployName;
        public string animatorUndeployName;
        public float animatorDeployLength;
        public List<ParticleSystem> shotParticles;
        public List<AudioSource> shotAudios;

        private Animator animator;
        private Coroutine coroutine;  

        private void Awake()
        {
            animator = GetComponent<Animator>();
            
        }

        public void ShotMachineGun()
        {
            if(coroutine != null) return;

            coroutine = StartCoroutine(_ShotMachineGun());
        }
        
        private IEnumerator _ShotMachineGun()  
        {  
            animator.SetTrigger(animatorDeployName);

            yield return new WaitForSeconds(animatorDeployLength);

            for (int i = 0; i < shotParticles.Count; i++)
            {
                 shotParticles[i].Play();
                 shotAudios[Random.Range(0, shotAudios.Count)].Play();
                 yield return new WaitForSeconds(0.1f);
            }
            
            for (int i = 0; i < shotParticles.Count; i++)
            {
                shotParticles[i].Play();
                shotAudios[Random.Range(0, shotAudios.Count)].Play();
                yield return new WaitForSeconds(0.1f);
            }
            
            animator.SetTrigger(animatorUndeployName);

            yield return new WaitForSeconds(animatorDeployLength);
            StopCoroutine();

        }

        private void StopCoroutine()
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }
    
}
