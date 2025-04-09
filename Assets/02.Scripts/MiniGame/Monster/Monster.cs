using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jun.Monster
{
    public class Monster : MonsterBase
    {
        Damage _damage;
        List<PlayableCharacter> targets;

        public override void EndTurn()
        {
            base.EndTurn();
            MiniGameScenesManager.instance.Success -= OnSuccess;
            MiniGameScenesManager.instance.Fail -= OnFail;
            MiniGameScenesManager.instance.Parring -= OnParrying;
        }
        void OnSuccess()
        {
            if (!IsMyTurn) return;
            EndTurn();
            
        }

        void OnFail()
        {
            if (!IsMyTurn) return;
            foreach (PlayableCharacter target in targets)
            {
                target.TakeDamage(_damage);
            }
            EndTurn();
        }

        void OnParrying()
        {
            if (!IsMyTurn) return;
            TakeDamage(_damage);
            EndTurn();
        }
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
            _damage = new Damage(DamageType.Melee, AttackPower, gameObject);
            ExecuteAttack(SkillRange.Single, "Attack");
        }

        protected override void Skill1()
        {
            base.Skill1();
            SkillDataSO skill1 = _skillComponent.skillDataList[0];
            float damageAmount = AttackPower * skill1.SkillMultiplier;
            _damage = new Damage(skill1.DamageType, damageAmount, gameObject);
            ExecuteAttack(skill1.SkillRange, "Skill1");
        }

        protected override void Skill2()
        {
            base.Skill2();
            SkillDataSO skill2 = _skillComponent.skillDataList[1];
            float damageAmount = AttackPower * skill2.SkillMultiplier;
            _damage = new Damage(skill2.DamageType, damageAmount, gameObject);
            ExecuteAttack(skill2.SkillRange, "Skill2");
        }

        void ExecuteAttack(SkillRange range, string animName)
        {
            targets = range == SkillRange.Single ? new List<PlayableCharacter> { _target } : new List<PlayableCharacter>(_playableCharacters);
            List<PlayableCharacter> dyingTargets = new List<PlayableCharacter>();

            foreach (PlayableCharacter target in targets)
            {
                if (target.WouldDieFromAttack(_damage))
                {
                    dyingTargets.Add(target);
                    Debug.Log(target.gameObject.name + " is attacking " + target.gameObject.name + ".");
                }
            }

            bool anyWillDie = dyingTargets.Count > 0;

            StartCoroutine(PlayAnimationAndProcessTargets(animName, targets, anyWillDie));
        }


        IEnumerator PlayAnimationAndProcessTargets(string animName, List<PlayableCharacter> targets, bool anyWillDie)
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
                
                StartCoroutine(MiniGameScenesManager.instance.Transition.MiniGameTransition()); // 수정
                Time.timeScale = 0.2f;
                yield return new WaitForSecondsRealtime(1f); // 수정
                Time.timeScale = 1f;

                // 가장 먼저 죽을 타겟만 지정 (또는 목록 저장해도 OK)
                // MiniGameScenesManager.instance.player = targets.Find(t => t.WouldDieFromAttack(_damage)).gameObject;
                MiniGameScenesManager.instance.StartMiniGame(_damage.Type);
                MiniGameScenesManager.instance.Success += OnSuccess;
                MiniGameScenesManager.instance.Fail += OnFail;
                MiniGameScenesManager.instance.Parring += OnParrying;

                // EndTurn();
                
            } else
            {
                Debug.Log("아무도 죽지 않음");
                foreach (PlayableCharacter target in targets)
                {
                    target.TakeDamage(_damage);
                }
                EndTurn();
            }
        }
    }
}
