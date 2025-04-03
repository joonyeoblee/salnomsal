using UnityEngine;

public enum DamageType
{
    Melee, // 근거리
    Ranged, // 원거리
    Magic // 마법
}

public struct Damage
{
    public DamageType Type;
    public float Value;
    public GameObject DamageFrom;

    public Damage(DamageType type, float value, GameObject damageFrom)
    {
        Type = type;
        Value = value;
        DamageFrom = damageFrom;
    }
}
