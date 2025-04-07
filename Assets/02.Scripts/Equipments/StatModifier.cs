using System;

namespace Equipment
{
    [Serializable]
    public class StatModifier
    {
        public StatType StatType;
        public float Value;

        public StatModifier(StatType statType, float value)
        {
            StatType = statType;
            Value = value;
        }
    }
}