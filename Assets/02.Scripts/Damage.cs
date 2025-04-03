using UnityEngine;

public enum DamageType
{
    Melee, // �ٰŸ�
    Ranged, // ���Ÿ�
    Magic // ����
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
