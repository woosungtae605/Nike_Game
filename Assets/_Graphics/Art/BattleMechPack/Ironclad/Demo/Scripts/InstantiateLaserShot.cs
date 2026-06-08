// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections;
using UnityEngine;

namespace PampelArt.Characters.Ironclad.Demo
{
    public class InstantiateLaserShot : MonoBehaviour
    {
        public GameObject laserShot;
        public float minWaitingTime = 1f;
        
        private bool wait;

        public void StartLaserShot()
        {
            if (wait) return;
            Instantiate(laserShot, gameObject.transform.position, gameObject.transform.rotation);
            wait = true;
            StartCoroutine(_Wait());
        }
        
        private IEnumerator _Wait()
        {
            yield return new WaitForSeconds(minWaitingTime);
            wait = false;
        }
        
    }
}