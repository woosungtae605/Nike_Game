using LitMotion;
using LitMotion.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Camera
{
    public class CameraMove : MonoBehaviour
    {
        [SerializeField] private float duration = 0.5f;
        private MotionHandle _moveHandle;
        public void MoveToNekke(Transform target)
        {
            _moveHandle.TryCancel();
            _moveHandle = LMotion.Create(transform.position, target.position,duration).WithEase(Ease.Linear).Bind(x => transform.position = x);
        }
    }
}