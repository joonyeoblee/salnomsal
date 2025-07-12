using System;
using System.Collections.Generic;
using Equipment;
public enum EquipmentType
{
    Weapon,
    Armor
}
public class EquipmentInstance
{
    public EquipmentType Type;
    public string IconAddress;
    public string BorderAddress;
    public Rarity Rarity;
    public List<StatModifier> BaseStats;
    public List<AppliedPassiveEffect> AppliedPassives;

    public EquipmentInstance(
        EquipmentType type,
        string iconAddress,
        string borderAddress,
        Rarity rarity,
        List<StatModifier> baseStats,
        List<AppliedPassiveEffect> appliedPassives)
    {
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
