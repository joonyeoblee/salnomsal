using NUnit.Framework;
using System;
using UnityEngine;
using System.Collections.Generic;

public abstract class Character : MonoBehaviour
{
	public DamageType DamageType;
	public float MaxHealth;
	public float MaxCost;
	public float CostRegen;
	public float AttackPower;
	public float CriticalChance;
	public float CriticalDamage;
	public int Resistance;
	public bool HasBuff;
	public bool IsDefending;

	protected float _health;
	public float CurrentHealth => _health;
	protected float _cost;
	public float Cost => _cost;
	
	public Action OnTurnStart;
	public Action OnTurnEnd;
	public abstract void Register();
	public abstract void DoAction(SkillSlot slot, List<ITargetable> targets);
	public abstract void Death(DamageType type);
}
