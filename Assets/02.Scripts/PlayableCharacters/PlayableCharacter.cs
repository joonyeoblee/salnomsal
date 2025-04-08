using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum SkillSlot
{
	DefaultAttack,
	Skill1,
	Skill2,

	None
}

public class PlayableCharacter : Character, ITurnActor, ITargetable
{
	public string CharacterName;
    public List<Skill> Skills;

    public TargetType _targetType;
    public TargetType TargetType
    {
        get => _targetType;
        set => _targetType = value;
    }

    private bool _isAlive;
    public bool IsAlive => _isAlive;

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

    private void Start()
    {
        _health = MaxHealth;
        _cost = MaxCost;
        _isAlive = true;
        _currentSpeed = BasicSpeed;
    }

    public override void Register()
	{

	}

    public void ResetAfterBattle()
    {
        gameObject.transform.DOScale(1f, 0.5f);
        _cost = MaxCost;
        _currentSpeed = BasicSpeed;

        while (OnTurnEnd != null)
        {
            OnTurnStart?.Invoke();
            OnTurnEnd?.Invoke();
        }
    }

    public void CostGain()
    {
        _cost += CostRegen;
        _cost = Mathf.Min(_cost, MaxCost);
    }

	public void StartTurn()
	{
        gameObject.transform.DOScale(1.2f, 0.5f);
        Debug.Log($"{CharacterName}: Playable Turn Start");
        CombatManager.Instance.CurrentActor = this;
        // UI로 캐릭터 정보 전송
		OnTurnStart?.Invoke();
	}

	public void EndTurn()
	{
        gameObject.transform.DOScale(1f, 0.5f);
        Debug.Log($"{CharacterName}: Playable Turn End");
        OnTurnEnd?.Invoke();
		CombatManager.Instance.EndTurn(this);
    }

	public override void DoAction(SkillSlot slot, List<ITargetable> targets)
	{
		// 스킬 이펙트 시전
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Attack"); // 트리거 배열 만들어서 슬롯따라서 실행
        }

        foreach (ITargetable target in targets)
		{
			Skills[(int)slot].UseSkill(this, target);
        }
        EndTurn();
    }

	public override void Death(DamageType type)
	{
        _isAlive = false;
        Debug.Log("Death");
	}

	public void TakeDamage(Damage damage)
	{
		_health -= damage.Value;
		if (_health <= 0)
		{
			Death(damage.Type);
        }
		else
		{
			Debug.Log($"{gameObject.name} Took {damage.Value} damage from {damage.DamageFrom.name}. Remaining health: {_health}");
		}
	}
	public bool WouldDieFromAttack(Damage damage)
	{
		float _copyCurrentHealth = _health;
		_copyCurrentHealth -= damage.Value;
		if (_copyCurrentHealth <= 0)
		{
			return true;
		}
		return false;
	}

    public void GetBuff(Buff buff)
    {
        Debug.Log($"버프 사용. {buff.BuffStatType} : {buff.BuffMultiplier}");
        switch (buff.BuffStatType)
        {
            case BuffStatType.AttackPower:
                AttackPower *= buff.BuffMultiplier;
                break;
            case BuffStatType.CriticalChance:
                CriticalChance += buff.BuffMultiplier;
                break;
            case BuffStatType.CriticalDamage:
                CriticalDamage += buff.BuffMultiplier;
                break;
            case BuffStatType.Taunt:
                Taunt = true;
                break;
        }

        HasBuff = true;

        OnTurnStart += buff.TickBuff;
        OnTurnEnd += buff.RemoveBuff;
    }

	public void RemoveBuff(Buff buff)
	{
        Debug.Log($"버프 해제. {buff.BuffStatType} : {buff.BuffMultiplier}");
        switch (buff.BuffStatType)
        {
            case BuffStatType.AttackPower:
                AttackPower /= buff.BuffMultiplier;
                break;
            case BuffStatType.CriticalChance:
                CriticalChance -= buff.BuffMultiplier;
                break;
            case BuffStatType.CriticalDamage:
                CriticalDamage -= buff.BuffMultiplier;
                break;
            case BuffStatType.Taunt:
                Taunt = false;
                break;
        }

        OnTurnStart -= buff.TickBuff;
        OnTurnEnd -= buff.RemoveBuff;

        if (OnTurnEnd == null)
        {
            HasBuff = false;
        }
    }

    public void GetHeal(float amount)
    {
		Debug.Log($"체력회복. {amount}");
        _health += amount;
		_health = Mathf.Min(_health, MaxHealth);
    }
}
