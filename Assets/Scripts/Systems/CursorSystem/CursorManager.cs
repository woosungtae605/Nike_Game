using UnityEngine;

namespace Systems.CursorSystem
{
    public class CursorManager : MonoBehaviour
    {
        [SerializeField] private Texture2D cursorTexture;

        private void Start()
        {
            if (cursorTexture == null)
            {
                ResetToDefault();
                return;
            }

            Vector2 hotSpot = new Vector2(cursorTexture.width / 2f, cursorTexture.height / 2f);
            Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public static void ResetToDefault()
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
