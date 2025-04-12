using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Equipment;
using UnityEngine;
using Debug = UnityEngine.Debug;
public enum SkillSlot
{
	DefaultAttack,
	Skill1,
	Skill2,

	None
}

[Serializable]
public class PlayableCharacter : Character, ITurnActor, ITargetable
{
	public string CharacterName;
	public int Index;
	public int Immune;
	public List<Skill> Skills;
	public List<GameObject> SkillEffects;
	public List<GameObject> HitEffects;
	public TargetType _targetType;
	public TargetType TargetType
	{
		get => _targetType;
		set => _targetType = value;
	}

	[SerializeField] private GameObject _model;
	[SerializeField] GameObject _muzzle;
	[SerializeField] GameObject Projectile;
	public GameObject Model { get => _model.gameObject; }

	private Vector3 cameraOriginPosition;
	float cameraOriginSize = 5f;
	public EquipmentSaveData Weapon;
	public EquipmentSaveData Armor;

	private bool _isAlive;
	public bool IsAlive => _isAlive;
	public int IsStun { get; set; }

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

	Vector3 OriginPosition;
	public float moveDuration = 0.5f;

	[SerializeField] Animator _animator;
	readonly Dictionary<StatType, float> finalStats = new Dictionary<StatType, float>();

	private void Start()
	{
		_health = MaxHealth;
		_cost = MaxCost;
		_isAlive = true;
		_currentSpeed = BasicSpeed;

		OriginPosition = transform.position;

		_animator = GetComponentInChildren<Animator>();

		ApplyItems();
		cameraOriginPosition = new Vector3(0, 0, -10);
	}

	void ApplyItems()
	{

		if (Weapon != null)
		{
			foreach (StatModifier stat in Weapon.BaseStats)
			{
				if (!finalStats.ContainsKey(stat.StatType))
					finalStats[stat.StatType] = 0;

				finalStats[stat.StatType] += stat.Value;

			}
		}

		if (Armor != null)
		{
			foreach (StatModifier stat in Armor.BaseStats)
			{
				if (!finalStats.ContainsKey(stat.StatType))
					finalStats[stat.StatType] = 0;

				finalStats[stat.StatType] += stat.Value;
			}
		}
		ApplyStats(finalStats);
	}

	void ApplyStats(Dictionary<StatType, float> finalStats)
	{
		foreach (KeyValuePair<StatType, float> stat in finalStats)
		{
			switch (stat.Key)
			{
				case StatType.Attack:
					AttackPower += stat.Value;
					break;

				case StatType.MaxHealth:
					MaxHealth += stat.Value;
					break;

				case StatType.MaxMana:
					MaxCost += stat.Value;
					break;

				case StatType.Speed:
					BasicSpeed += (int)stat.Value;
					break;
			}
		}
	}

	public void ApplyStat(float health, float cost, float attack, int speed)
	{
		MaxHealth = health;
		MaxCost = cost;
		AttackPower = attack;
		BasicSpeed = speed;
	}

	public void GetImmune()
	{
		Immune += 2;
	}

	public void ResetAfterBattle()
	{
		int loopSafeCount = 20; // 매직넘버

		gameObject.transform.DOScale(1f, 0.5f);
		_cost = MaxCost;
		_currentSpeed = BasicSpeed;

		while (OnTurnEnd != null && loopSafeCount > 0)
		{
			OnTurnStart?.Invoke();
			OnTurnEnd?.Invoke();
			--loopSafeCount;
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
		CostGain();
		OnTurnStart?.Invoke();
		// UI로 캐릭터 정보 전송
		UI_Battle.Instance.BattleUI[Index].Refresh(this);
		UI_Battle.Instance.SwitchUI(Index);
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
		_cost -= Skills[(int)slot].SkillCost;
		UI_Battle.Instance.BattleUI[Index].Refresh(this);
		
		Skill currentSkill = Skills[(int)slot];
		if (currentSkill.SkillData.SkillType == SkillType.Attack && currentSkill.IsMelee)
		{
			Vector3 vecc = new Vector3(CombatManager.Instance.PlayerAttackPosition.position.x + 2, CombatManager.Instance.PlayerAttackPosition.position.y + 2, -10);

			Sequence sequence = DOTween.Sequence();
			sequence.Append(Camera.main.DOOrthoSize(4f, 1f)).SetEase(Ease.OutCubic);
			sequence.Join(Camera.main.transform.DOMove(vecc, 1f).SetEase(Ease.OutQuad));
			sequence.Join(transform.DOMove(CombatManager.Instance.PlayerAttackPosition.position, moveDuration).SetEase(Ease.InOutQuad));

			sequence.OnComplete((
				) =>
			{
				Debug.Log($"{vecc}: vecc ");
				StartCoroutine(DoActionCoroutine(slot, targets));
			});
		} else if (currentSkill.SkillData.SkillType == SkillType.Attack && !currentSkill.IsMelee)
		{
			MonoBehaviour target = targets[targets.Count - 1] as MonoBehaviour;
			Vector3 vecc = new Vector3((transform.position.x + target.gameObject.transform.position.x) / 2, -0.5f, -10);

			Sequence sequence = DOTween.Sequence();
			sequence.Append(Camera.main.DOOrthoSize(4.5f, 1f)).SetEase(Ease.OutCubic);
			sequence.Join(Camera.main.transform.DOMove(vecc, 1f).SetEase(Ease.OutQuad));
			sequence.OnComplete((
			) =>
			{
				Debug.Log($"{vecc}: vecc ");
				StartCoroutine(DoActionCoroutine(slot, targets));
			});
		} else if (currentSkill.SkillData.SkillType == SkillType.Heal || currentSkill.SkillData.SkillType == SkillType.Buff)
		{
			Vector3 vecc = new Vector3(-3, -1, -10);

			Sequence sequence = DOTween.Sequence();
			sequence.Append(Camera.main.DOOrthoSize(4f, 1f)).SetEase(Ease.OutCubic);
			sequence.Join(Camera.main.transform.DOMove(vecc, 1f).SetEase(Ease.OutQuad));
			sequence.OnComplete((
			) =>
			{
				Debug.Log($"{vecc}: vecc ");
				StartCoroutine(DoActionCoroutine(slot, targets));
			});
		}
		else
		{
			// 이동이 끝난 다음에 코루틴 시작
			StartCoroutine(DoActionCoroutine(slot, targets));
		}
	}

	IEnumerator DoActionCoroutine(SkillSlot slot, List<ITargetable> targets)
	{
		string animName = "";

		switch (slot)
		{
			case SkillSlot.DefaultAttack:
				animName = "Attack";

				_animator.SetTrigger(animName);
				break;
			case SkillSlot.Skill1:
				animName = "Skill1";
				_animator.SetTrigger(animName);
				break;
			case SkillSlot.Skill2:
				animName = "Skill2";

				_animator.SetTrigger(animName);
				break;
		}

	
		// 애니메이션 끝날 때까지 대기
		yield return StartCoroutine(WaitAnimationEnd(animName, slot, targets));


		// 다시 원래 위치로 이동한 다음, EndTurn 

		Sequence seq = DOTween.Sequence();
		seq.AppendInterval(0.2f);
		seq.Append(transform.DOMove(OriginPosition, moveDuration).SetEase(Ease.InOutQuad));
		seq.Join(Camera.main.DOOrthoSize(cameraOriginSize, 0.5f).SetEase(Ease.OutCubic));
		seq.Join(Camera.main.transform.DOMove(cameraOriginPosition, 0.5f).SetEase(Ease.OutCubic));
		seq.OnComplete(() =>
		{
			EndTurn();
		});
	}

	IEnumerator WaitAnimationEnd(string animName, SkillSlot slot, List<ITargetable> targets)
	{
		// 트리거 입력 후, 해당 애니메이션 시작될 때까지 대기
		AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(0);
		while (!info.IsName(animName))
		{
			yield return null;
			info = _animator.GetCurrentAnimatorStateInfo(0);
		}
		if (Projectile != null && Skills[(int)slot].HasProjectile)
		{
			foreach (ITargetable target in targets)
			{
				GameObject _projectile = Instantiate(Projectile);
				_projectile.transform.position = _muzzle.transform.position;
				_projectile.transform.DOMove(target.Model.transform.position, 0.5f);
			}

		}
		// Instantiate(SkillEffects[(int)slot], transform.position, Quaternion.identity);
		// 애니메이션 재생 끝날 때까지 대기
		while (info.normalizedTime < 1f)
		{
			yield return null;
			info = _animator.GetCurrentAnimatorStateInfo(0);
		}


		
		
		// 스킬 사용 처리
		foreach (ITargetable target in targets)
		{
			MonoBehaviour mb = target as MonoBehaviour;

			if (HitEffects.Count > 0)
			{
				Instantiate(HitEffects[(int)slot], mb.transform.position, mb.transform.rotation);
			}

			Skills[(int)slot].UseSkill(this, target);
		}
	}

	public override void Death(DamageType type)
	{
        _isAlive = false;

        PlayerPrefs.DeleteKey(GameManager.Instance.Teams[Index]);
        PlayerPrefs.Save();
	}

	public void TakeDamage(Damage damage)
	{
		if (Immune > 0)
		{
			Immune -= 1;
			damage.Value = 0f;
			FloatingTextDisplay.Instance.ShowFloatingText(Model.transform.position, "Immune", FloatingTextType.Immune);
        }
		_health -= damage.Value;
		_health = Mathf.Max(_health, 0);
		UI_Battle.Instance.BattleUI[Index].Refresh(this);
        UI_Battle.Instance.PartyHealthIndicator.RefreshHealth(this);

        if (_health <= 0)
		{
			Death(damage.Type);
		}
		else
		{
			Debug.Log($"{gameObject.name} Took {damage.Value} damage from {damage.DamageFrom.name}. Remaining health: {_health}");
			_animator.SetTrigger("Hit");
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
				Taunt += 1;
				break;
		}

		HasBuff = true;

		UI_Battle.Instance.BattleUI[Index].Refresh(this);
		UI_Battle.Instance.PartyHealthIndicator.RefreshHealth(this);

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
				Taunt -= 1;
				break;
		}

		OnTurnStart -= buff.TickBuff;
		OnTurnEnd -= buff.RemoveBuff;

		UI_Battle.Instance.BattleUI[Index].Refresh(this);
        UI_Battle.Instance.PartyHealthIndicator.RefreshHealth(this);

        if (OnTurnEnd == null)
		{
			HasBuff = false;
		}
	}

	public void GetHeal(float amount)
	{
		Debug.Log($"Before: {_health} + {amount}");
		_health += amount;
		Debug.Log($"After: {_health}");
        _health = Mathf.Min(_health, MaxHealth);
        UI_Battle.Instance.BattleUI[Index].Refresh(this);
		UI_Battle.Instance.PartyHealthIndicator.RefreshHealth(this);
    }
}
