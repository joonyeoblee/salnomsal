using System;

namespace Equipment
{
    [Serializable]
    public class StatRange
    {
        public StatType StatType;
        public float MinValue;
        public float MaxValue;
        public StatRange(StatType type, int i, int i1)
        {
            StatType = type;
            MinValue = i;
            MaxValue = i1;
        }
    }
}