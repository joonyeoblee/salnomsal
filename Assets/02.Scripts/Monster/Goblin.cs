using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jun.Monster
{
    public class Goblin : MonsterBase
    {
        protected override void Start()
        {
            base.Start();
            _health = MaxHealth;
            _mana = MaxMana;

            List<Func<Character, int>> conditionalList = new()
            {
                target => target.CurrentHealth < target.MaxHealth * 0.3f ? 5 : 0, // Skill1
                target => target.HasBuff ? 10 : 0 // Skill2
            };

            _skillComponent.SetConditionalPriorities(conditionalList);
        }
        
        protected override void Attack()
        {
            base.Attack();

            if (_target == null) return;

            Damage damage = new Damage(DamageType.Melee, AttackPower, gameObject);
            EnterMiniGame(damage, "Attack");
        }

        protected override void Skill1()
        {
            base.Skill1();

            if (_target == null) return;

            float damageAmount = AttackPower * _skillComponent.skillDataList[0].SkillMultiplier;
            Damage damage = new Damage(DamageType.Melee, damageAmount, gameObject);
            EnterMiniGame(damage, "Skill1");
        }

        protected override void Skill2()
        {
            base.Skill2();

            if (_target == null) return;

            float damageAmount = _skillComponent.skillDataList[1].SkillMultiplier;
            Damage damage = new Damage(DamageType.Magic, damageAmount, gameObject);
            EnterMiniGame(damage, "Skill2");
        }

        void EnterMiniGame(Damage damage, string skillName)
        {
            if (_target.WouldDieFromAttack(damage))
            {
                Time.timeScale = 0.2f;
                StartCoroutine(WaitForAnimationEndAndEndTurn(skillName));
                MiniGameScenesManager.instance.player = _target.gameObject;
                MiniGameScenesManager.instance.ChangeSceneToMiniGameMagic();
            }
            else
            {
                _target.TakeDamage(damage);
                StartCoroutine(WaitForAnimationEndAndEndTurn(skillName));
            }
        }

        IEnumerator WaitForAnimationEndAndEndTurn(string animName)
        {
            yield return null;

            AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(0);

            while (!info.IsName(animName))
            {
                yield return null;
                info = _animator.GetCurrentAnimatorStateInfo(0);
            }

            while (info.normalizedTime < 1f)
            {
                yield return null;
                info = _animator.GetCurrentAnimatorStateInfo(0);
            }

            EndTurn(); // ✅ 애니메이션이 끝난 후에 실행됨
        }

        
    }
}
