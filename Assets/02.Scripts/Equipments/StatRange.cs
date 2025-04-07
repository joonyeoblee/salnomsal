using System;

namespace Equipment
{
    [Serializable]
    public class StatRange
    {
        public StatType StatType;
        public float MinValue;
        public float MaxValue;
    }
}