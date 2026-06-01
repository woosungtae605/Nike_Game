using UnityEngine;

namespace Systems.CursorSystem
{
    public class CursorManager : MonoBehaviour
    {
        [SerializeField] private Texture2D cursorTexture;
        private int _cursorWidth;
        private void Start()
        {
            Vector2 hotSpot = new Vector2(cursorTexture.width / 2f, cursorTexture.height / 2f);
            _cursorWidth = cursorTexture.width;
            Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
        }
    }
}