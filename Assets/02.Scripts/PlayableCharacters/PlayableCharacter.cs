using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

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
        _mana = MaxMana;
        _isAlive = true;
        _currentSpeed = BasicSpeed;
    }


    public override void Register()
	{

	}

	public void StartTurn()
	{
		Debug.Log($"{CharacterName}: Playable Turn Start");
        CombatManager.Instance.CurrentActor = this;
		OnTurnStart?.Invoke();
	}

	public void EndTurn()
	{
        Debug.Log($"{CharacterName}: Playable Turn End");
        OnTurnEnd?.Invoke();
		CombatManager.Instance.EndTurn(this);
    }

	public override void DoAction(SkillSlot slot, List<ITargetable> targets)
	{
		// 스킬 이펙트 시전

		foreach (ITargetable target in targets)
		{
			Skills[(int)slot].UseSkill(this, target);
        }
		OnTurnEnd?.Invoke();
        CombatManager.Instance.EndTurn(this);
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
			Debug.Log($"Took {damage.Value} damage from {damage.DamageFrom.name}. Remaining health: {_health}");
		}
	}

    public void GetBuff(float amount)
    {
		Debug.Log($"버프를 얻었습니다. {amount}");
    }

    public void GetHeal(float amount)
    {
		Debug.Log($"체력회복. {amount}");
        _health += amount;
		_health = Mathf.Min(_health, MaxHealth);
    }
}
