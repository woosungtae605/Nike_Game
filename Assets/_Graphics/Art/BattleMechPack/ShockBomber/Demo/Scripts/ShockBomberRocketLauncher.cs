// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PampelArt.Characters.ShockBomber.Demo
{
    
    public class ShockBomberRocketLauncher : MonoBehaviour
    {
        public float waitForOut = 1.5f;
        public float waitForShoot = 1.5f;
        public float waitForNext = 1.5f;
        
        public UnityEvent launcherDeploy;
        public UnityEvent launcherShoot;
        public UnityEvent launcherRetract;
        
        private void Start()
        {
            StartCoroutine(_ActivateRocketLauncher());
        }

        private IEnumerator _ActivateRocketLauncher()
        {
            for (;;)
            {
                launcherDeploy.Invoke();
                yield return new WaitForSeconds(waitForOut);
                launcherShoot.Invoke();
                yield return new WaitForSeconds(waitForShoot);
                launcherRetract.Invoke();
                yield return new WaitForSeconds(waitForOut);
                yield return new WaitForSeconds(waitForNext);
            }
        }
    }
    
}
