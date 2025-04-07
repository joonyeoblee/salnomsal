using System;
using System.Collections.Generic;

namespace Equipment
{
    [Serializable]
    public class EquipmentSaveData
    {
        public string Id;
        public Rarity Rarity;
        public List<StatModifier> BaseStats;
        public List<AppliedPassiveEffect> AppliedPassives;

        public EquipmentSaveData(string itemId, EquipmentInstance instance)
        {
            Id = itemId;
            Rarity = instance.Rarity;
            BaseStats = instance.BaseStats;
            AppliedPassives = instance.AppliedPassives;
        }

        public EquipmentInstance ToInstance(EquipmentSO so)
        {
            return new EquipmentInstance(so, Rarity, BaseStats, AppliedPassives);
        }
    }

}