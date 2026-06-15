using System.Collections;
using Gamelib.ObjectPool.Runtime;
using UnityEngine;

namespace Systems
{
    public class ParticlePooling : PoolableMono
    {
        public PoolManagerSo PoolManagerSo { get; set; }

        private ParticleSystem _ps;
        private Coroutine _returnCoroutine;
        private bool _isReturned;

        private void Awake()
        {
            Init();
        }

        private void OnDisable()
        {
            if (_returnCoroutine != null)
            {
                StopCoroutine(_returnCoroutine);
                _returnCoroutine = null;
            }
        }

        public override void ResetItem()
        {
            Init();

            _isReturned = false;
            _ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            _ps.Clear(true);
            _ps.Play(true);

            if (_returnCoroutine != null)
                StopCoroutine(_returnCoroutine);

            _returnCoroutine = StartCoroutine(ReturnWhenFinished());
        }

        public void Play(Vector3 position)
        {
            transform.position = position;
            ResetItem();
        }

        public void Stop()
        {
            if (_ps == null)
                return;

            _ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        private IEnumerator ReturnWhenFinished()
        {
            yield return null;

            while (_ps != null && _ps.IsAlive(true))
                yield return null;

            _returnCoroutine = null;
            ReturnToPool();
        }

        private void ReturnToPool()
        {
            if (_isReturned)
                return;

            _isReturned = true;

            if (PoolManagerSo == null)
                PoolManagerSo = GetComponentInParent<PoolInitializer>()?.PoolManager;

            if (PoolManagerSo != null)
                PoolManagerSo.Push(this);
            else
                gameObject.SetActive(false);
        }

        private void Init()
        {
            if (_ps != null)
                return;

            _ps = GetComponent<ParticleSystem>();

            if (_ps == null)
                _ps = GetComponentInChildren<ParticleSystem>();

            Debug.Assert(_ps != null, $"{nameof(ParticlePooling)} needs a ParticleSystem.", this);
        }
    }
}
