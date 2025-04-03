using System;
using System.Collections.Generic;
using Jun.Skill;
using UnityEngine;

namespace Jun.Monster
{
    public class Ogre : MonsterBase
    {
        public bool IsMyTurn;
        MonsterSkill _skillComponent;

        void Start()
        {
            _skillComponent = GetComponent<MonsterSkill>();

            _health = MaxHealth;
            _mana = MaxMana;

            List<Func<Character, int>> conditionalList = new List<Func<Character, int>>
            {
                target => target.CurrentHealth < target.MaxHealth * 0.3f ? 5 : 0, // Skill1
                target => target.IsDefending ? 10 : 0 // Skill2
            };

            _skillComponent.SetConditionalPriorities(conditionalList);
        }

        void Update()
        {
            if (IsMyTurn)
            {
                IsMyTurn = false;

                if (_skillComponent == null || _target == null)
                {
                    Debug.LogWarning("스킬 또는 타겟이 없음, 기본 공격");
                    Attack();
                    return;
                }

                SkillDecision decision = _skillComponent.ChooseSkillWithIndex(_target);
                Debug.Log(decision.Skill == null);
                if (decision == null || decision.Skill == null)
                {
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
        }

        protected override void Attack()
        {
            base.Attack();

            Damage damage = new Damage(DamageType.Melee, AttackPower, gameObject);
            _target.TakeDamage(damage);
        }

        protected override void Skill1()
        {
            base.Skill1();

            Damage damage = new Damage(DamageType.Melee, AttackPower * _skillComponent.skillDataList[0].SkillMultiplier, gameObject); // 예시 값
            _target.TakeDamage(damage);
        }

        protected override void Skill2()
        {
            base.Skill2();

            Damage damage = new Damage(DamageType.Magic, _skillComponent.skillDataList[1].SkillMultiplier, gameObject); // 예시 값
            _target.TakeDamage(damage);
        }

        public override void StartTurn()
        {
            base.StartTurn();

            IsMyTurn = true;
            _target = GetTarget();
        }
    }
}
