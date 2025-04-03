using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public DamageType DamageType;
    public float MaxHealth;
    public float MaxMana;
    public float AttackPower;
    public float CriticalChance;
    public float CriticalDamage;
    public int Speed;
    public int Resistance;

    protected float _health;
    public float CurrentHealth => _health;

    protected float _mana;

    protected abstract void Attack(Character target);
    protected abstract void Skill1(Character target);
    protected abstract void Skill2(Character target);
    protected abstract void Death(DamageType type);

    public abstract void TakeDamage(Damage damage);
}
