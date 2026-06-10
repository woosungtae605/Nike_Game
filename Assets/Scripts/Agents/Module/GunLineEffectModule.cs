using System.Collections;
using Agents.Players;
using Module;
using UnityEngine;

namespace Agents.Module
{
    [RequireComponent(typeof(LineRenderer))]
    public class GunLineEffectModule : MonoBehaviour, IModule
    {
        private Agent _agent;
        private LineRenderer _lineRenderer;
        
        private Coroutine _coroutine;
        public void Initialize(ModuleOwner owner)
        {
            _agent = owner as Agent;
            _lineRenderer = GetComponent<LineRenderer>();
        }

        public void Shot(float duration, Vector3 startPos, Vector3 lastPos)
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            AddLine(startPos, lastPos);
            _coroutine = StartCoroutine(ResetAfterDelay(duration));
        }

        private IEnumerator ResetAfterDelay(float duration)
        {
            yield return new WaitForSeconds(duration);
            Reset();
        }

        private void AddLine(Vector3 startPos, Vector3 lastPos)
        {
            int startIndex = _lineRenderer.positionCount;
            _lineRenderer.positionCount = startIndex + 2;
            _lineRenderer.SetPosition(startIndex, startPos);
            _lineRenderer.SetPosition(startIndex + 1, lastPos);
        }

        private void Reset()
        {
            _lineRenderer.positionCount = 0;
            _coroutine = null;
        }
    }
}