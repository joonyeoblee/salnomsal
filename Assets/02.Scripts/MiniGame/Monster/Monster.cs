using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Jun.Monster
{
    public class Monster : MonsterBase
    {
        Damage _damage;
        List<PlayableCharacter> targets;
        Vector3 OriginPosition;
        public float moveDuration = 0.5f;

        // ReSharper disable Unity.PerformanceAnalysis
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
            ReturnToOrigin(() => EndTurn());
            
        }

        // ReSharper disable Unity.PerformanceAnalysis
        void OnFail()
        {
            if (!IsMyTurn) return;
            foreach (PlayableCharacter target in targets)
            {
                target.TakeDamage(_damage);
            }
            ReturnToOrigin(() => EndTurn());
        }

        void OnParrying()
        {
            if (!IsMyTurn) return;
            TakeDamage(_damage);
            ReturnToOrigin(() => EndTurn());
        }

        // ReSharper disable Unity.PerformanceAnalysis
        void ReturnToOrigin(Action onComplete = null)
        {
            transform.DOMove(OriginPosition, moveDuration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => { onComplete?.Invoke(); });
        }
        
        protected override void Start()
        {
            base.Start();
            _health = MaxHealth;
            _mana = MaxMana;

            OriginPosition = transform.position;
            
            List<Func<Character, int>> conditionalList = new List<Func<Character, int>>
            {
                target => target.CurrentHealth < target.MaxHealth * 0.3f ? 5 : 0,
                target => target.HasBuff ? 10 : 0
            };

            _skillComponent.SetConditionalPriorities(conditionalList);
        }

        // ReSharper disable Unity.PerformanceAnalysis
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
                  
            transform.DOMove(CombatManager.Instance.EnemyAttackPosition.position, moveDuration).SetEase(Ease.OutQuad);

            targets = range == SkillRange.Single ? new List<PlayableCharacter> { _target } : new List<PlayableCharacter>(_playableCharacters);

            Debug.Log("🎯 타겟 개수: " + targets.Count);

            List<PlayableCharacter> dyingTargets = new List<PlayableCharacter>();

            foreach (PlayableCharacter target in targets)
            {
                if (target == null)
                {
                    Debug.LogError("❌ target is null!");
                    continue;
                }

                if (target.WouldDieFromAttack(_damage))
                {
                    dyingTargets.Add(target);
                }
            }

            Debug.Log("☠ 죽을 타겟 수: " + dyingTargets.Count);

            bool anyWillDie = dyingTargets.Count > 0;

            Debug.Log("▶ PerformSkillRoutine 실행");
            StartCoroutine(PerformSkillRoutine(animName, targets, anyWillDie));
        }


        IEnumerator PerformSkillRoutine(string animName, List<PlayableCharacter> targets, bool anyWillDie)
        {
            yield return StartCoroutine(WaitForAnimation(animName));

            if (anyWillDie)
            {
                Time.timeScale = 0.2f;
                yield return StartCoroutine(MiniGameScenesManager.instance.Transition.MiniGameTransition());
                yield return new WaitForSecondsRealtime(1f);
                Time.timeScale = 1f;
                yield return new WaitForSeconds(0.3f);

                MiniGameScenesManager.instance.StartMiniGame(_damage.Type);
                MiniGameScenesManager.instance.Success += OnSuccess;
                MiniGameScenesManager.instance.Fail += OnFail;
                MiniGameScenesManager.instance.Parring += OnParrying;
            } else
            {
                foreach (PlayableCharacter target in targets)
                {
                    target.TakeDamage(_damage);
                }
                transform.DOMove(OriginPosition, moveDuration).SetEase(Ease.OutQuad).OnComplete(() => { EndTurn(); });
            }
        }
        IEnumerator WaitForAnimation(string animName)
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
        }
    }
}
