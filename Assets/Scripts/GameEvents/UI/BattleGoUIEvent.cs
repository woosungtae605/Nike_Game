using CoreSystem.BusSystem;
using Systems.GameSystem.Wave;

namespace GameEvents.UI
{
    public struct BattleGoUIEvent : IEvent
    {
        public readonly WaveInformationSO waveInformationSo;

        public BattleGoUIEvent(WaveInformationSO waveInformationSo)
        {
            this.waveInformationSo = waveInformationSo;
        }
    }
}