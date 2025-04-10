using System;
using System.Collections.Generic;
using Jun.Skill;
using UnityEngine;

namespace Jun.Monster
{
    class TargetCandidate
    {
        public PlayableCharacter Character;
        public int priority;
    }

    public class MonsterBase : EnemyCharacter
    {
        public Animator _animator;
        protected MonsterSkill _skillComponent;
        protected List<PlayableCharacter> _playableCharacters => CombatManager.Instance.PlayableCharacter;
        PlayableCharacter _lastTarget;
        protected PlayableCharacter _target;
        public bool IsMyTurn;

        protected virtual void Start()
        {
            _animator = GetComponentInChildren<Animator>();
            _skillComponent = GetComponent<MonsterSkill>();
            IsAlive = true;
        }

        protected void ChoiceSkill()
        {
            if (!IsMyTurn) return;

            if (_skillComponent == null)
            {
                Debug.LogWarning($"{name}: 스킬 컴포넌트 없음 → 기본 공격");
                Attack();
                return;
            }

            SkillDecision decision = _skillComponent.ChooseSkillWithTarget(_playableCharacters);

            if (decision == null || decision.Skill == null)
            {
                Debug.Log($"{name}: 선택된 스킬 없음 → 기본 공격");
                Attack();
                return;
            }

            _target = (PlayableCharacter)decision.Target;
            _lastTarget = _target;

            int index = decision.Index;

            List<Action> skillActions = new List<Action> { Skill1, Skill2, Skill3, Skill4 };

            if (index >= 0 && index < skillActions.Count)
            {
                Debug.Log($"{index + 1}번 스킬 실행");
                skillActions[index].Invoke();
            } else
            {
                Attack();
            }
        }

        public override void StartTurn()
        {
            IsMyTurn = true;
            ChoiceSkill();
        }

        protected override void Attack()
        {
            _animator.Play("Attack");
        }

        protected override void Skill1()
        {
            _animator.Play("Skill1");
        }

        protected override void Skill2()
        {
            _animator.Play("Skill2");
        }

        protected override void Skill3()
        {
            _animator.Play("Skill3");
        }

        protected override void Skill4()
        {
            _animator.Play("Skill4");
        }

        protected override void Register()
        {
            CombatManager.Instance.Monsters.Add(this);
        }

        protected override void Death(DamageType type)
        {
            Debug.Log("Death");
            IsAlive = false;
            CombatManager.Instance.Monsters.Remove(this);
            Destroy(gameObject);
        }

        public override void TakeDamage(Damage damage)
        {
            _animator.Play("Hit");
            _health -= damage.Value;
            if (_health <= 0)
            {
                Death(damage.Type);
            } else
            {
                Debug.Log($"{gameObject.name}Took {damage.Value} damage from {damage.DamageFrom.name}. Remaining health: {_health}");
            }
        }

        public override void EndTurn()
        {
            IsMyTurn = false;
            CombatManager.Instance.EndTurn(this);
            Debug.Log($"{name} EndTurn");
        }

        public void OnDamage()
        {
            // Placeholder
        }
    }
}
