using CoreSystem.BusSystem;
using UnityEngine;

namespace GameEvents.UI
{
    public struct CursorImageChangeEvent : IEvent
    {
        public readonly Sprite CursorSprite;
        
        public CursorImageChangeEvent(Sprite cursor)
        {
            CursorSprite = cursor;
        }
    }
}