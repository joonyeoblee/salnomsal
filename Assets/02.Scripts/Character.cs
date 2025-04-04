using System;
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
	public bool HasBuff;
	public bool IsDefending;

	protected float _health;
	public float CurrentHealth => _health;
	protected float _mana;
	public float Mana => _mana;
	
	public Action OnTurnStart;
	public Action OnTurnEnd;
	public abstract void Register();
	public abstract void DoAction();
	public abstract void Death(DamageType type);

	public abstract void TakeDamage(Damage damage);
}
