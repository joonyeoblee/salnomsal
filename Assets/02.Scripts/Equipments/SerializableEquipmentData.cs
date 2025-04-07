using System;
using System.Collections.Generic;
using Equipment;

[Serializable]
public class SerializableEquipmentData
{
    public string Id;
    public string EquipmentName;
    public Rarity Rarity;
    public List<StatModifier> BaseStats;
    public List<AppliedPassiveEffect> AppliedPassives;
}