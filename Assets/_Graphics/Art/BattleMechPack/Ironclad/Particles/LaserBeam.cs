// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections;
using UnityEngine;

namespace PampelArt.Characters
{
    public class LaserBeam : MonoBehaviour
    {
        public LineRenderer beam;
        public GameObject laserPoint;
        [Min(0)] public LayerMask raycastLayers;
        public float duration = 2.25f;
        public float maxLength = 10f;
        public float startWidth = 0.2f;
        public float endWidth = 0.2f;

        private GameObject laserPointStart;
        private GameObject laserPointEnd;

        private void Awake()
        {
            beam.startWidth = startWidth;
            beam.endWidth = endWidth;

            beam.enabled = false;
        }


        public void StartLaser()
        {
            if (beam.enabled) return;
            beam.enabled = true;

            laserPointStart = Instantiate(laserPoint, transform.position, transform.localRotation);

            beam.SetPosition(0, gameObject.transform.position);
            
            if (Physics.Raycast(transform.position, transform.forward, out var hit,
                    maxLength, raycastLayers))
            {
                laserPointEnd = Instantiate(laserPoint, hit.point - transform.forward * 0.1f, transform.localRotation);
                beam.SetPosition(1, hit.point);
            }
            else
            {
                beam.SetPosition(1, gameObject.transform.position + gameObject.transform.forward * maxLength);
            }
                

            StartCoroutine(_Laser());
        }

        private IEnumerator _Laser()
        {
            yield return new WaitForSeconds(duration);
            beam.enabled = false;
            Destroy(laserPointStart);
            if (laserPointEnd != null) Destroy(laserPointEnd);
        }
    }
}