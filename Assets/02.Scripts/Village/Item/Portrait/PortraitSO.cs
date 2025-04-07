using UnityEngine;
[CreateAssetMenu(fileName = "PortraitSO", menuName = "Scriptable Objects/PortraitSO")]
public class PortraitSO : ScriptableObject
{
    public DamageType DamageType;
    public float MaxHealth;
    public float MaxMana;
    public float AttackPower;
    public float CriticalChance;
    public float CriticalDamage;
    public int Resistance;
}
