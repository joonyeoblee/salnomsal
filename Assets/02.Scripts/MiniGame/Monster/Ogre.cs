using System;
using System.Collections.Generic;

namespace Jun.Monster
{
    public class Ogre : MonsterBase
    {
        protected override void Start()
        {
            base.Start();
            
            _health = MaxHealth;
            _mana = MaxMana;

            List<Func<Character, int>> conditionalList = new()
            {
                target => target.CurrentHealth < target.MaxHealth * 0.3f ? 5 : 0, // Skill1
                target => target.IsDefending ? 10 : 0 // Skill2
            };

            _skillComponent.SetConditionalPriorities(conditionalList);
        }

        protected override void Attack()
        {
            base.Attack();

            if (_target == null) return;

            Damage damage = new Damage(DamageType.Melee, AttackPower, gameObject);
            _target.TakeDamage(damage);
        }

        protected override void Skill1()
        {
            base.Skill1();

            if (_target == null) return;

            float damageAmount = AttackPower * _skillComponent.skillDataList[0].SkillMultiplier;
            Damage damage = new Damage(DamageType.Melee, damageAmount, gameObject);
            _target.TakeDamage(damage);
        }

        protected override void Skill2()
        {
            base.Skill2();

            if (_target == null) return;

            float damageAmount = _skillComponent.skillDataList[1].SkillMultiplier;
            Damage damage = new Damage(DamageType.Magic, damageAmount, gameObject);
            _target.TakeDamage(damage);
        }
    }
}
