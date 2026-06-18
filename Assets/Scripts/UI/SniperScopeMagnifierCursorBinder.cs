using Reflex.Attributes;
using Systems;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Graphic))]
    public class SniperScopeMagnifierCursorBinder : MonoBehaviour
    {
        [SerializeField, Inject] private PlayerInputSO playerInputSO;
        [SerializeField] private string centerPropertyName = "_Center";

        private Graphic _graphic;
        private Material _runtimeMaterial;
        private int _centerPropertyId;

        private void Awake()
        {
            _graphic = GetComponent<Graphic>();
            _centerPropertyId = Shader.PropertyToID(centerPropertyName);

            if (_graphic.material != null)
            {
                _runtimeMaterial = Instantiate(_graphic.material);
                _graphic.material = _runtimeMaterial;
            }
        }

        private void OnDestroy()
        {
            if (_runtimeMaterial != null)
                Destroy(_runtimeMaterial);
        }

        private void LateUpdate()
        {
            if (_runtimeMaterial == null)
                return;

            Vector2 mousePosition = GetMousePosition();
            Vector2 center = new Vector2(
                Screen.width > 0 ? mousePosition.x / Screen.width : 0.5f,
                Screen.height > 0 ? mousePosition.y / Screen.height : 0.5f);

            _runtimeMaterial.SetVector(_centerPropertyId, new Vector4(center.x, center.y, 0f, 0f));
        }

        private Vector2 GetMousePosition()
        {
            if (playerInputSO != null)
                return playerInputSO.CurrentMousePosition;

            return Mouse.current != null ? Mouse.current.position.ReadValue() : Vector2.zero;
        }
    }
}
