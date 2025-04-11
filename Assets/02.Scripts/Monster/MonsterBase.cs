using System;
using System.Collections.Generic;
using System.Linq;
using Jun.Skill;
using UnityEngine;

namespace Jun.Monster
{
    class TargetCandidate {
        public PlayableCharacter Character;
        public int priority;
    }
    
    public class MonsterBase : EnemyCharacter
    {
        protected SkillDecision decision;
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
            
            if (IsStun)
            {
                IsStun = false;
                Debug.Log("Stun");
                EndTurn();
            }
            
        }
        protected void ChoiceSkill()
        {
            if (!IsMyTurn) return;

            if (_skillComponent == null || _target == null)
            {
                Debug.LogWarning($"{name}: 스킬 또는 타겟이 없음 → 기본 공격");
                Attack();
                return;
            }

            decision = _skillComponent.ChooseSkillWithIndex(_target);

            if (decision == null || decision.Skill == null)
            {
                Debug.Log($"{name}: 선택된 스킬이 없음 → 기본 공격");
                Attack();
                return;
            }

            int index = decision.Index;

            // 스킬 인덱스에 따른 실행 함수 목록
            List<Action> skillActions = new List<Action> { Skill1, Skill2, Skill3, Skill4 };
                
            if (index >= 0 && index < skillActions.Count)
            {
                Debug.Log($"{index +1 }스킬 실행됌");
                skillActions[index].Invoke();
            } else
            {
                Attack();
            }
        }
        public override void StartTurn()
        {
            IsMyTurn = true;
            _target = GetTarget();
            _lastTarget = _target; // 다음 타겟 우선도 계산용
            Debug.Log(gameObject.name);
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
            _health = Mathf.Max(_health, 0);
            UI_Battle.Instance.EnemyHealthIndicator.RefreshHealth(this);
            if (_health <= 0)
            {
                Death(damage.Type);
            } else
            {
                Debug.Log($"{gameObject.name}Took {damage.Value} damage from {damage.DamageFrom.name}. Remaining health: {_health}");
            }
        }
        
        // 타켓을 설정하는 알고리즘
        TargetCandidate EvaluateTarget(PlayableCharacter Character)
        {
            int priority = 0;

            // 도발 무조건 때림
            if (Character.Taunt > 0) priority += 200;
            
            if (Character.CurrentHealth < Character.MaxHealth * 0.3f) priority += 10;
            if (Character.DamageType == DamageType.Magic) priority += 7;
            if (Character.HasBuff) priority += 5;
            if (Character.IsDefending) priority -= 5;
            if (Character == _lastTarget) priority -= 10;
          
            
            return new TargetCandidate { Character = Character, priority = priority };
        }

        PlayableCharacter ChooseTarget(List<PlayableCharacter> playerCharacters)
        {
            List<TargetCandidate> candidates = new();
            foreach (var Character in playerCharacters) {
                if (Character.IsAlive)
                {
                    candidates.Add(EvaluateTarget(Character));
                }
            }

            return candidates.OrderByDescending(c => c.priority).First().Character;
        }

        public override void EndTurn()
        {
            IsMyTurn = false;
            CombatManager.Instance.EndTurn(this);
            Debug.Log($"{name} EndTurn");
        }

        protected PlayableCharacter GetTarget()
        {
            return ChooseTarget(_playableCharacters);
        }
        public void OnDamage()
        {
            
        }
      
    }

}