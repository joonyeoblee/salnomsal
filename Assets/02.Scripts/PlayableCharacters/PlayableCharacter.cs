using UnityEngine;

public enum SkillSlot
{
	DefaultAttack,
	Skill1,
	Skill2,
	None
}

namespace PlayableCharacters
{
	// 플레이어 캐릭터를 나타내는 클래스입니다.
	// Character 클래스를 상속받아 구현합니다.
	// 플레이어 캐릭터는 공격, 스킬 사용, 사망 등의 기능을 가집니다.
	// 플레이어 캐릭터는 Damage를 받아 체력을 감소시키고, 체력이 0 이하가 되면 사망합니다.
	public class PlayableCharacter : Character
	{
		public string CharacterName;
		public SkillDataSO[] Skills;

		private SkillSlot _sellectedSkill;

		private Character[] _targets;

		protected override void Register()
		{

		}

		public override void StartTurn()
		{
			OnTurnStart?.Invoke();
		}

		public void SetSellectedSkill(SkillSlot slot)
		{
			_sellectedSkill = slot;
		}

		public void GetTarget()
		{
			// 스킬 타입에 맞게 _targets를 설정합니다.
			if (Skills[(int)_sellectedSkill].SkillRange == SkillRange.Single)
			{
				Debug.Log("단일 타겟 스킬");
			}
			else if (Skills[(int)_sellectedSkill].SkillRange == SkillRange.Global)
			{
				Debug.Log("전체 타겟 스킬");
			}
		}

		public void DoAction()
		{
			if (_mana < Skills[(int)_sellectedSkill].SkillCost)
			{
				Debug.Log("마나가 부족합니다.");
				return;
			}

			GetTarget();
			foreach (Character target in _targets)
			{

			}
		}

		protected override void Attack()
		{
			if (_targets == null)
			{
				Debug.Log("타겟이 없습니다.");
				return;
			}

			foreach (Character target in _targets)
			{
				if (target == null)
				{
					Debug.Log("타겟이 없습니다.");
				}
			}
		}

		protected override void Skill1()
		{
			Debug.Log("Skill1");
		}

		protected override void Skill2()
		{
			Debug.Log("Skill2");
		}

		protected override void Death(DamageType type)
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
}
