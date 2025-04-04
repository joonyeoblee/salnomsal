using System;

namespace Equipment
{
    [Serializable]
    public class StatModifier
    {
        public StatType statType;
        public float Value;
    }

    public enum StatType
    {
        Attack,
        Defense,
        Speed,
        CritChance,
        CritDamage,
        Accuracy,
        Evasion,
        MaxHealth,
        MaxMana,
        Resistance
    }
}