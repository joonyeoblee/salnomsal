using System.Collections.Generic;

namespace Equipment
{
    public class EquipmentInstance
    {
        public EquipmentSO Template { get; }
        public Rarity Rarity { get; }
        public List<StatModifier> BaseStats { get; }
        public List<AppliedPassiveEffect> AppliedPassives { get; }

        public EquipmentInstance(
            EquipmentSO template,
            Rarity rarity,
            List<StatModifier> baseStats,
            List<AppliedPassiveEffect> appliedPassives
        )
        {
            Template = template;
            Rarity = rarity;
            BaseStats = baseStats;
            AppliedPassives = appliedPassives;
        }
    }
}