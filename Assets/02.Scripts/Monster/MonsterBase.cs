using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Jun.Monster
{
    class TargetCandidate {
        public Character Character;
        public int priority;
    }
    
    public class MonsterBase : Character
    {
        public Animator _animator;
        List<Character> _playableCharacters => CombatManager.Instance.PlayableCharacter;
        Character _lastTarget;
        protected Character _target;
        void OnEnable()
        {
            Register();
        }
        void Start()
        {
            _animator = GetComponentInChildren<Animator>();

            
        }

        protected override void Register()
        {
            CombatManager.Instance.Monsters.Add(this);
        }
        public override void StartTurn()
        {
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
        TargetCandidate EvaluateTarget(Character Character) {
            int priority = 0;

            if (Character.CurrentHealth < Character.MaxHealth * 0.3f) priority += 10;
            if (Character.DamageType == DamageType.Magic) priority += 7;
            if (Character.HasBuff) priority += 5;
            if (Character.IsDefending) priority -= 5;
            if (Character == _lastTarget) priority += 3;

            return new TargetCandidate { Character = Character, priority = priority };
        }
        
        Character ChooseTarget(List<Character> playerCharacters) {
            List<TargetCandidate> candidates = new();
            foreach (var Character in playerCharacters) {
                candidates.Add(EvaluateTarget(Character));
            }

            return candidates.OrderByDescending(c => c.priority).First().Character;
        }

        protected Character GetTarget()
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