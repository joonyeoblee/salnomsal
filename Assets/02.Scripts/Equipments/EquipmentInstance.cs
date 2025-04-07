using System;
using System.Collections.Generic;

namespace Equipment
{
    public class EquipmentInstance
    {
        public string Id;
        public EquipmentSO Template { get; }
        public List<StatModifier> BaseStats { get; }
        public List<AppliedPassiveEffect> AppliedPassives { get; }

        public EquipmentInstance(
            EquipmentSO template,
            List<StatModifier> baseStats,
            List<AppliedPassiveEffect> appliedPassives)
        {
            Id = Guid.NewGuid().ToString();
            
            Template = template;
            BaseStats = baseStats;
            AppliedPassives = appliedPassives;
        }
    }
}