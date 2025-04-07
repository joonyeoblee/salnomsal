using System;
using System.Collections.Generic;

namespace Equipment
{
    [Serializable]
    public class EquipmentSaveData
    {
        public string Id;
        public string EquipmentSOName;
        public Rarity Rarity;
        public List<StatModifier> BaseStats;
        public List<AppliedPassiveEffect> AppliedPassives;

        public EquipmentSaveData(EquipmentInstance equipmentInstance)
        {
            Id = equipmentInstance.Id;
            EquipmentSOName = equipmentInstance.Template.name;
            Rarity = equipmentInstance.Template.Rarity;
            BaseStats = equipmentInstance.BaseStats;
            AppliedPassives = equipmentInstance.AppliedPassives;
        }
    }
}