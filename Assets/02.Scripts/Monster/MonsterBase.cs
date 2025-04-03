using System;
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
        // Action monsterDelegate;
        Animator _animator;
        List<Character> _playableCharacters => CombatManager.Instance.PlayableCharacter;
        Character _lastTarget;
        void Start()
        {
            _animator = GetComponent<Animator>();
        }

        protected override void Register()
        {
            throw new NotImplementedException();
        }
        public override void StartTurn()
        {
            throw new NotImplementedException();
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
            
        }

        public override void TakeDamage(Damage damage)
        {
            
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

        public Character GetTarget( )
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