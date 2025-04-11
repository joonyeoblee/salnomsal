using System;
using UnityEngine;
public abstract class EnemyCharacter : MonoBehaviour, ITurnActor, ITargetable
{
	[SerializeField] private GameObject _model;
    public GameObject Model { get => _model.gameObject; }
	public DamageType DamageType;
	public int Index;
	public float MaxHealth;
	public float MaxMana;
	public float AttackPower;
	public float CriticalChance;
	public float CriticalDamage;
	public int Resistance;
	public bool HasBuff;
	public bool IsDefending;
	public bool IsStun;
	public TargetType _targetType;
	public TargetType TargetType {
		get => _targetType;
		set => _targetType = value; 
	}

    protected float _health;
	public float CurrentHealth => _health;
	protected float _mana;
	public float Mana => _mana;

	private bool _isAlive;
    public bool IsAlive
	{
		get => _isAlive;
		set => _isAlive = value;
    }

	[SerializeField] private int _basicSpeed;
    public int BasicSpeed
	{
		get => _basicSpeed; 
		set => _basicSpeed = value; 
	}
	private int _currentSpeed;
	public int CurrentSpeed
	{
		get => _currentSpeed;
		set => _currentSpeed = value;
    }

    public Action OnTurnStart;
	public Action OnTurnEnd;
	protected abstract void Register();
	public abstract void StartTurn();
	protected abstract void Attack();
	protected abstract void Skill1();
	protected abstract void Skill2();
	protected abstract void Skill3();
	protected abstract void Skill4();
	protected abstract void Death(DamageType type);

	public abstract void TakeDamage(Damage damage);

	public abstract void EndTurn();
  
    public void GetBuff(Buff buff)
    {
        throw new NotImplementedException();
    }

    public void RemoveBuff(Buff buff)
    {
        throw new NotImplementedException();
    }

    public void GetHeal(float amount)
    {
        throw new NotImplementedException();
    }
   

}
