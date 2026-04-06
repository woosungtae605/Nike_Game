using System;

namespace CoreSystem
{
    public static class Bus<T> where T : IEvent
    {
        public static event Action<T> OnEvent;
        
        public static void Raise(T evt)  => OnEvent?.Invoke(evt);
    }
}