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
        public Animator _animator;
        protected MonsterSkill _skillComponent;
        List<PlayableCharacter> _playableCharacters => CombatManager.Instance.PlayableCharacter;
        PlayableCharacter _lastTarget;
        protected PlayableCharacter _target;
        public bool IsMyTurn;
        void OnEnable()
        {
            Register();
        }
        protected virtual void Start()
        {
            _animator = GetComponentInChildren<Animator>();
            _skillComponent = GetComponent<MonsterSkill>();

        }
        protected virtual void Update()
        {
            if (!IsMyTurn) return;

            IsMyTurn = false;

            if (_skillComponent == null || _target == null)
            {
                Debug.LogWarning($"{name}: 스킬 또는 타겟이 없음 → 기본 공격");
                Attack();
                return;
            }

            SkillDecision decision = _skillComponent.ChooseSkillWithIndex(_target);

            Debug.Log(decision == null);
            Debug.Log(decision.Skill == null);
            if (decision == null || decision.Skill == null)
            {
                Debug.Log($"{name}: 선택된 스킬이 없음 → 기본 공격");
                Attack();
                return;
            }

            switch (decision.Index)
            {
            case 0:
                Skill1();
                break;
            case 1:
                Skill2();
                break;
            default:
                Attack();
                break;
            }
        }

        protected override void Register()
        {
            CombatManager.Instance.Monsters.Add(this);
        }
        public override void StartTurn()
        {
            IsMyTurn = true;
            _target = GetTarget();
            _lastTarget = _target; // 다음 타겟 우선도 계산용
            
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
            } else
            {
                Debug.Log($"{gameObject.name}Took {damage.Value} damage from {damage.DamageFrom.name}. Remaining health: {_health}");
            }
        }
        
        // 타켓을 설정하는 알고리즘
        TargetCandidate EvaluateTarget(PlayableCharacter Character)
        {
            int priority = 0;

            if (Character.CurrentHealth < Character.MaxHealth * 0.3f) priority += 10;
            if (Character.DamageType == DamageType.Magic) priority += 7;
            if (Character.HasBuff) priority += 5;
            if (Character.IsDefending) priority -= 5;
            if (Character == _lastTarget) priority += 3;

            return new TargetCandidate { Character = Character, priority = priority };
        }

        PlayableCharacter ChooseTarget(List<PlayableCharacter> playerCharacters)
        {
            List<TargetCandidate> candidates = new();
            foreach (var Character in playerCharacters) {
                candidates.Add(EvaluateTarget(Character));
            }

            return candidates.OrderByDescending(c => c.priority).First().Character;
        }

        protected PlayableCharacter GetTarget()
        {
            return ChooseTarget(_playableCharacters);
        }
        public void OnDamage()
        {
            
        }
        public void OnDeath()
        {
            
        }
    }

}