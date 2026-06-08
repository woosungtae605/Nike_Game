// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace PampelArt.Characters.ShockBomber.Demo
{
    public class ShockBomberMachineGun : MonoBehaviour
    {
        [Tooltip("Gap between the animations.")]
        public float timeGap = 1.5f;
        
        [Tooltip("Gap between the shots of the animation.")]
        public float gapLight = 0.12f;
        [Tooltip("Gap between the shots of the animation.")]
        public float gapHeavy = 0.3f;

        public UnityEvent onMachineGunLightFirst;
        public UnityEvent onMachineGunLightSecond;
        public UnityEvent onMachineGunLightThird;
        public UnityEvent onMachineGunLightFourth;


        public UnityEvent onMachineGunHeavy01First;
        public UnityEvent onMachineGunHeavy01Second;

        
        public UnityEvent onMachineGunHeavy02;

        private void Start()
        {
            StartCoroutine(_ShotGuns());
        }


        private IEnumerator _ShotGuns()
        {
            for (;;)
            {
                onMachineGunLightFirst.Invoke();
                yield return new WaitForSeconds(gapLight);
                onMachineGunLightSecond.Invoke();
                yield return new WaitForSeconds(gapLight);
                onMachineGunLightThird.Invoke();
                yield return new WaitForSeconds(gapLight);
                onMachineGunLightFourth.Invoke();
                
                yield return new WaitForSeconds(timeGap);
                
                onMachineGunHeavy01First.Invoke();
                yield return new WaitForSeconds(gapHeavy);
                onMachineGunHeavy01Second.Invoke();

                yield return new WaitForSeconds(timeGap);
                
                onMachineGunHeavy02.Invoke();
                
                yield return new WaitForSeconds(timeGap);
            }
        }
    }
}