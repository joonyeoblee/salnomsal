using System;
using System.Collections.Generic;
using Equipment;

public class EquipmentInstance
{
    public string Name;
    public EquipmentType Type;
    public string IconAddress;
    public string BorderAddress;
    public Rarity Rarity;
    public List<StatModifier> BaseStats;
    public List<AppliedPassiveEffect> AppliedPassives;

    public EquipmentInstance(
        string name,
        EquipmentType type,
        string iconAddress,
        string borderAddress,
        Rarity rarity,
        List<StatModifier> baseStats,
        List<AppliedPassiveEffect> appliedPassives)
    {
        Name = name;
        Type = type;
        IconAddress = iconAddress;
        BorderAddress = borderAddress;
        Rarity = rarity;
        BaseStats = baseStats;
        AppliedPassives = appliedPassives;
    }

    public EquipmentSaveData ToSaveData()
    {
        return new EquipmentSaveData
        {
            Name = Name,
            Type = Type,
            IconAddress = IconAddress,
            BorderAddress = BorderAddress,
            Rarity = Rarity,
            BaseStats = BaseStats,
            AppliedPassives = AppliedPassives
        };
    }

    public static EquipmentInstance FromSaveData(EquipmentSaveData data)
    {
        return new EquipmentInstance(
            data.Name,
            data.Type,
            data.IconAddress,
            data.BorderAddress,
            data.Rarity,
            data.BaseStats,
            data.AppliedPassives
        );
    }
}
[Serializable]
public class EquipmentSaveData
{
    public string Name;
    public EquipmentType Type;
    public string IconAddress;
    public string BorderAddress;
    public Rarity Rarity;
    public List<StatModifier> BaseStats;
    public List<AppliedPassiveEffect> AppliedPassives;
}
