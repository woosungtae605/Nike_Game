using System.Collections;
using Gamelib.ObjectPool.Runtime;
using UnityEngine;

namespace Agents.Missiles
{
    public class BaseMissile : Missile
    {
        [SerializeField] private float curveHeight = 3f;
        [SerializeField] private float curveSideOffset = 2f;
        [SerializeField] private float arriveDistance = 0.1f;

        private Coroutine _moveCoroutine;


        public override void Shot(Vector3 startPos, Vector3 targetPos)
        {
            if (_moveCoroutine != null)
                StopCoroutine(_moveCoroutine);

            transform.position = startPos;
            _moveCoroutine = StartCoroutine(MoveBezierRoutine(startPos, targetPos));
        }

        private IEnumerator MoveBezierRoutine(Vector3 startPos, Vector3 targetPos)
        {
            Vector3 controlPoint = GetControlPoint(startPos, targetPos);
            float distance = Vector3.Distance(startPos, targetPos);
            float duration = distance / MissileSo.Speed;

            float elapsed = 0f;
            Vector3 previousPosition = startPos;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                Vector3 nextPosition = EvaluateQuadraticBezier(startPos, controlPoint, targetPos, t);
                transform.position = nextPosition;

                Vector3 moveDirection = nextPosition - previousPosition;
                if (moveDirection.sqrMagnitude > 0.0001f)
                    transform.rotation = Quaternion.LookRotation(moveDirection.normalized);

                previousPosition = nextPosition;

                if (Vector3.Distance(nextPosition, targetPos) <= arriveDistance)
                    break;

                yield return null;
            }

            transform.position = targetPos;
            _moveCoroutine = null;
            Hit();
        }

        private void Hit()
        {
            if (PoolManagerSo != null)
                PoolManagerSo.Push(this);
        }

        private Vector3 GetControlPoint(Vector3 startPos, Vector3 targetPos)
        {
            Vector3 controlPoint = (startPos + targetPos) * 0.5f;
            controlPoint += Vector3.up * curveHeight;

            Vector3 direction = targetPos - startPos;
            Vector3 sideDirection = Vector3.Cross(Vector3.up, direction).normalized;
            if (sideDirection.sqrMagnitude <= 0.0001f)
                sideDirection = transform.right;

            controlPoint += sideDirection * curveSideOffset;
            return controlPoint;
        }

        private Vector3 EvaluateQuadraticBezier(Vector3 startPos, Vector3 controlPoint, Vector3 targetPos, float t)
        {
            Vector3 startToControl = Vector3.Lerp(startPos, controlPoint, t);
            Vector3 controlToTarget = Vector3.Lerp(controlPoint, targetPos, t);
            return Vector3.Lerp(startToControl, controlToTarget, t);
        }

    }
}