using UnityEngine;

namespace Agents.Enemies
{
    public class TurretController : MonoBehaviour
    {
        [Header("Components")]
        [Tooltip("Y축으로 회전시킬 포탑의 머리(자식 오브젝트)를 할당하세요.")]
        public Transform turretHead;

        [Tooltip("포탑이 조준할 타겟 오브젝트를 할당하세요.")]
        public Transform target;

        [Header("Settings")]
        [Tooltip("포탑의 회전 속도를 조절합니다.")]
        public float turnSpeed = 10f;

        private void LateUpdate()
        {
            if (turretHead == null || target == null)
            {
                return;
            }

            Vector3 directionToTarget = target.position - turretHead.position;
            directionToTarget.y = 0f;

            if (directionToTarget.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                Quaternion smoothedRotation = Quaternion.Slerp(turretHead.rotation, targetRotation, turnSpeed * Time.deltaTime);
                Vector3 currentLocalEulerAngles = turretHead.localEulerAngles;

                turretHead.rotation = smoothedRotation;
                float newLocalY = turretHead.localEulerAngles.y;

                turretHead.localEulerAngles = new Vector3(currentLocalEulerAngles.x, newLocalY, currentLocalEulerAngles.z);
            }
        }
    }
}