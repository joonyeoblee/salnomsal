using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public float MaxHealth;
    public float MaxMana;
    public float AttackPower;
    public float CriticalChance;
    public float CriticalDamage;
    public int Speed;
    public int Resistance;

    protected float _health;
    protected float _mana;

    protected abstract void Attack();
    protected abstract void Skill1();
    protected abstract void Skill2();
    protected abstract void Death();

    public abstract void TakeDamage(Damage damage);
}
