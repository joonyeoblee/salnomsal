using System;
using System.Collections.Generic;
using UnityEngine;
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

// 저장시 key value 
// id key value

// 게임시작 
// 너 필드에 있어 그럼 나 한번 생겼었구나
// 저장공간에서 내 id 검색해야지
// id 불변 
// Load id -> 대입
// 장비 구조체

// _saveData => 장비 구조체
// => AttackPower => _saveData.AttackPower