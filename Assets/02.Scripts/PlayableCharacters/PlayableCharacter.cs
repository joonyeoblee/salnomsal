using Jun;
using UnityEngine;

public enum SkillSlot
{
	DefaultAttack,
	Skill1,
	Skill2,
	None
}

public class PlayableCharacter : Character, ITurnActor
{
	public string CharacterName;
	public SkillDataSO[] Skills;

	private SkillSlot _selectedSkill;
	private EnemyCharacter[] _targets;

	public int BasicSpeed { get; set; }
	private int _currentSpeed;
	public int CurrentSpeed
	{
		get => _currentSpeed;
		set => _currentSpeed = value;
	}

	public override void Register()
	{

	}

	public void StartTurn()
	{
		CombatManager.Instance.CurrentActor = this;
		OnTurnStart?.Invoke();
	}

	public void EndTurn()
	{
	}

	public void SetSellectedSkill(SkillSlot slot)
	{
		_selectedSkill = slot;
	}

	public void GetTarget()
	{
		// 스킬 타입에 맞게 _targets를 설정합니다.
		if (Skills[(int)_selectedSkill].SkillRange == SkillRange.Single)
		{
			Debug.Log("단일 타겟 스킬");
		}
		else if (Skills[(int)_selectedSkill].SkillRange == SkillRange.Global)
		{
			Debug.Log("전체 타겟 스킬");
		}
	}

	public override void DoAction()
	{
		if (_mana < Skills[(int)_selectedSkill].SkillCost)
		{
			Debug.Log("마나가 부족합니다.");
			return;
		}

		GetTarget();
		foreach (EnemyCharacter target in _targets)
		{
			Debug.Log("타겟에게 스킬을 시전합니다"); // 타겟 이름 생길 예정
		}
		OnTurnEnd?.Invoke();
	}

	public override void Death(DamageType type)
	{
		Debug.Log("Death");
	}

	public override void TakeDamage(Damage damage)
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
}
