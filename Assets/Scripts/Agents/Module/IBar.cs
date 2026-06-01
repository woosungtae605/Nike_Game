using System;

namespace Agents.Module
{
    public interface IBar
    {
        public int CurrentValue { get; }
        public int MaxValue { get; }
        public event Action<int, int> OnChanged;
    }
}