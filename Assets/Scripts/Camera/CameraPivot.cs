using Systems;
using UnityEngine;

namespace Camera
{
    public class CameraPivot : MonoBehaviour
    {
        
        [Header("Settings")]
        [SerializeField] private float maxCameraAngleY;
        [SerializeField] private float minCameraAngleY;
        [SerializeField] private float cameraAngleYSpeed;
        [SerializeField] private float maxCameraAngleX;
        [SerializeField] private float minCameraAngleX;
        [SerializeField] private float cameraAngleXSpeed;
        
        private float _currentXRotation = 0f;
        private float _currentYRotation = 0f;

        public void HandleMouseDeltaPos(Vector2 obj)
        {
            _currentYRotation += obj.x * cameraAngleYSpeed;
            _currentYRotation = Mathf.Clamp(_currentYRotation, minCameraAngleY, maxCameraAngleY);
            
            _currentXRotation += -obj.y  * cameraAngleXSpeed;
            _currentXRotation = Mathf.Clamp(_currentXRotation, minCameraAngleX, maxCameraAngleX);

            transform.localRotation = Quaternion.Euler(_currentXRotation, _currentYRotation, 0);
        }
    }
}