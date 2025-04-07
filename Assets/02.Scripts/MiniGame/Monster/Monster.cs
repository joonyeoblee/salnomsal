using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jun.Monster
{
    public class Monster : MonsterBase
    {
        protected override void Start()
        {
            base.Start();
            _health = MaxHealth;
            _mana = MaxMana;

            List<Func<Character, int>> conditionalList = new List<Func<Character, int>>
            {
                target => target.CurrentHealth < target.MaxHealth * 0.3f ? 5 : 0,
                target => target.HasBuff ? 10 : 0
            };

            _skillComponent.SetConditionalPriorities(conditionalList);
        }

        protected override void Attack()
        {
            base.Attack();
            ExecuteAttack(DamageType.Melee, AttackPower, SkillRange.Single, "Attack");
        }

        protected override void Skill1()
        {
            base.Skill1();
            float damageAmount = AttackPower * _skillComponent.skillDataList[0].SkillMultiplier;
            ExecuteAttack(DamageType.Melee, damageAmount, _skillComponent.skillDataList[0].SkillRange, "Skill1");
        }

        protected override void Skill2()
        {
            base.Skill2();
            float damageAmount = _skillComponent.skillDataList[1].SkillMultiplier;
            ExecuteAttack(DamageType.Magic, damageAmount, _skillComponent.skillDataList[1].SkillRange, "Skill2");
        }

        void ExecuteAttack(DamageType type, float amount, SkillRange range, string animName)
        {
            List<PlayableCharacter> targets = range == SkillRange.Single ? new List<PlayableCharacter> { _target } : new List<PlayableCharacter>(_playableCharacters);
            List<PlayableCharacter> dyingTargets = new List<PlayableCharacter>();

            foreach (PlayableCharacter target in targets)
            {
                Damage damage = new Damage(type, amount, gameObject);
                if (target.WouldDieFromAttack(damage))
                {
                    dyingTargets.Add(target);
                }
            }

            bool anyWillDie = dyingTargets.Count > 0;

            StartCoroutine(PlayAnimationAndProcessTargets(animName, targets, type, amount, anyWillDie));
        }


        IEnumerator PlayAnimationAndProcessTargets(string animName, List<PlayableCharacter> targets, DamageType type, float amount, bool anyWillDie)
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

            if (anyWillDie)
            {
                Time.timeScale = 0.2f;
                yield return new WaitForSecondsRealtime(0.2f);
                Time.timeScale = 1f;

                // 가장 먼저 죽을 타겟만 지정 (또는 목록 저장해도 OK)
                MiniGameScenesManager.instance.player = targets.Find(t => t.WouldDieFromAttack(new Damage(type, amount, gameObject))).gameObject;
                MiniGameScenesManager.instance.ChangeSceneToMiniGameMagic();
            } else
            {
                Debug.Log("아무도 죽지 않음");
                foreach (PlayableCharacter target in targets)
                {
                    Damage damage = new Damage(type, amount, gameObject);
                    target.TakeDamage(damage);
                }
                EndTurn();
            }
        }
    }
}
