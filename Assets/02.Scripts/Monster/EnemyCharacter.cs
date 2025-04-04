using System;
using UnityEngine;
public enum MapType
{
	Easy,
	Normal,
	Hard
}
public abstract class EnemyCharacter : MonoBehaviour
{
	public float _plusNum;
	public MapType MapType;
	public DamageType DamageType;
	[SerializeField] float _baseHealth;
	public float MaxHealth => _baseHealth + _plusNum;
	public float MaxMana;
	[SerializeField] float _baseAttackPower;
	public float AttackPower => _baseAttackPower + _plusNum;
	public float CriticalChance;
	public float CriticalDamage;
	public int Speed; // TODO: 스피드도 플러스 줄건지 알아야함
	public int Resistance;
	public bool HasBuff;
	public bool IsDefending;

	protected float _health;
	public float CurrentHealth => _health;
	protected float _mana;
	public float Mana => _mana;

    public int BasicSpeed { get; set; }

    public Action OnTurnStart;
	public Action OnTurnEnd;
	protected abstract void Register();
	public abstract void StartTurn();
	protected abstract void Attack();
	protected abstract void Skill1();
	protected abstract void Skill2();
	protected abstract void Death(DamageType type);

	public abstract void TakeDamage(Damage damage);

    public void EndTurn()
    {
    }
}
