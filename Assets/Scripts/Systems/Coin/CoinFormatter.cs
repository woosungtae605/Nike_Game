using UnityEngine;

namespace Systems.Coin
{
    public class CoinFormatter
    {
        public static string Format(long value)
        {
            if (value < 1_000L)              return value.ToString();
            if (value < 1_000_000L)          return $"{value / 1_000f:F1}K";
            if (value < 1_000_000_000L)      return $"{value / 1_000_000f:F1}M";
            if (value < 1_000_000_000_000L)  return $"{value / 1_000_000_000f:F1}B";
            return $"{value / 1_000_000_000_000d:F1}T";
        }
    }
}