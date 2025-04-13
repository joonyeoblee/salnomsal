using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Jun.Skill;
using UnityEngine;

namespace Jun.Monster
{
    public class Monster : MonsterBase
    {
        Damage _damage;
        List<PlayableCharacter> targets;
        Vector3 OriginPosition;
        public float moveDuration = 0.5f;
        List<PlayableCharacter> dyingTargets;
        public Transform Muzzle;

        public List<SkillConditionGroup> conditionGroups;

        public List<List<Func<Character, int>>> BuildConditionFunctions()
        {
            var result = new List<List<Func<Character, int>>>();

            foreach (SkillConditionGroup group in conditionGroups)
            {
                var funcGroup = new List<Func<Character, int>>();
                foreach (SkillCondition cond in group.conditions)
                {
                    funcGroup.Add(CreateConditionFunction(cond));
                }
                result.Add(funcGroup);
            }

            return result;
        }
        Func<Character, int> CreateConditionFunction(SkillCondition condition)
        {
            return target =>
            {
                switch (condition.conditionType)
                {
                    case ConditionType.LowHealth:
                        return target.CurrentHealth < target.MaxHealth * 0.3f ? condition.bonusScore : 0;
                    case ConditionType.HasBuff:
                        return target.HasBuff ? condition.bonusScore : 0;
                    case ConditionType.IsDefending:
                        return target.IsDefending ? condition.bonusScore : 0;
                    case ConditionType.HealthBelowX:
                        return target.CurrentHealth < condition.threshold ? condition.bonusScore : 0;
                    default:
                        return 0;
                }
            };
        }
        
        public override void EndTurn()
        {
            base.EndTurn();
            MiniGameScenesManager.Instance.Success -= OnSuccess;
            foreach (PlayableCharacter target in dyingTargets)
            {
                MiniGameScenesManager.Instance.Success -= target.GetImmune;
            }
            MiniGameScenesManager.Instance.Fail -= OnFail;
            MiniGameScenesManager.Instance.Parring -= OnParrying;
        }
        void OnSuccess()
        {
            if (!IsMyTurn) return;
            CleanupDyingTargets();
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
            EndTurn();
            DOTween.KillAll();
            TakeDamage(_damage);
            
            if(!IsAlive) return;
            ReturnToOrigin(() => EndTurn());
        }   
     
        
        void CleanupDyingTargets()
        {
            if (dyingTargets == null) return;

            foreach (var target in dyingTargets)
            {
                if (target != null)
                {
                    MiniGameScenesManager.Instance.Success -= target.GetImmune;
                }
            }
            dyingTargets.Clear();
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

            List<List<Func<Character, int>>> funcGroups = BuildConditionFunctions();
            _skillComponent.SetConditionalPriorities(funcGroups);
        }
        
        protected override void Attack()
        {
            if (DamageType == DamageType.Melee)
            {
                transform.DOMove(CombatManager.Instance.EnemyAttackPosition.position, moveDuration).SetEase(Ease.OutQuad).OnComplete(() => { base.Attack(); });
            }
            _damage = new Damage(DamageType, AttackPower, gameObject);
            ExecuteAttack(SkillRange.Single, "Attack");
        }

        protected override void Skill1()
        {
            
            SkillDataSO skill1 = _skillComponent.skillDataList[0];

            if (skill1.SkillType == SkillType.Attack && skill1.IsMelee)
            {
                transform.DOMove(CombatManager.Instance.EnemyAttackPosition.position, moveDuration).SetEase(Ease.OutQuad).OnComplete(() => { base.Skill1(); });
            } else
            {
                base.Skill1();
            }
            float damageAmount = AttackPower * skill1.SkillMultiplier;
            _damage = new Damage(skill1.DamageType, damageAmount, gameObject);
            ExecuteAttack(skill1.SkillRange, "Skill1");
        }

        protected override void Skill2()
        {
            SkillDataSO skill2 = _skillComponent.skillDataList[1];

            if (skill2.SkillType == SkillType.Attack && skill2.IsMelee)
            {
                transform.DOMove(CombatManager.Instance.EnemyAttackPosition.position, moveDuration).SetEase(Ease.OutQuad).OnComplete(() => { base.Skill2(); });
            } else
            {
                base.Skill2();
            }
            
            float damageAmount = AttackPower * skill2.SkillMultiplier;
            _damage = new Damage(skill2.DamageType, damageAmount, gameObject);
            ExecuteAttack(skill2.SkillRange, "Skill2");
        }
        void ExecuteAttack(SkillRange range, string animName)
        {
            Debug.Log("🟡 ExecuteAttack 진입");

            if (_target == null)
                Debug.LogWarning("⚠ _target이 null입니다");

            if (_playableCharacters == null)
                Debug.LogWarning("⚠ _playableCharacters가 null입니다");

            targets = range == SkillRange.Single ? new List<PlayableCharacter> { _target } : new List<PlayableCharacter>(_playableCharacters);

            Debug.Log("🎯 타겟 개수: " + (targets != null ? targets.Count.ToString() : "targets is null"));

            dyingTargets = new List<PlayableCharacter>();

            foreach (PlayableCharacter target in targets)
            {
                if (target == null)
                {
                    Debug.LogError("❌ target is null!");
                    continue;
                }

                if (target.Immune <= 0 && target.WouldDieFromAttack(_damage))
                {
                    dyingTargets.Add(target);
                }
            }

            foreach (PlayableCharacter dyingTarget in dyingTargets)
            {
                MiniGameScenesManager.Instance.Success += dyingTarget.GetImmune;
            }

            Debug.Log("☠ 죽을 타겟 수: " + dyingTargets.Count);

            if (animName != "Attack")
            {
                _mana -= decision.Skill.SkillData.SkillCost;
            }

            StartCoroutine(PerformSkillRoutine(animName, targets, dyingTargets.Count > 0, animName == "Attack"));
        }

        IEnumerator PerformSkillRoutine(string animName, List<PlayableCharacter> targets, bool anyWillDie, bool isBasicAttack)
        {
            if (!isBasicAttack && decision.Skill.SkillData.HasProjectile)
            {
                foreach (PlayableCharacter target in targets)
                {
                    Vector3 targetPosition = target.Model.transform.position;
                    GameObject _gameObject = Instantiate(decision.Skill.SkillData.ProjectilePrefab);
                    _gameObject.transform.position = Muzzle != null ? Muzzle.position : Model.transform.position;
                    _gameObject.transform.DOMove(targetPosition, moveDuration).SetEase(Ease.InOutSine);
                }
            }
            if (!isBasicAttack && decision.Skill.SkillData.HasProjectile)
            {
                _audioSource.clip = decision.Skill.SkillData.SkillSound;
                _audioSource.Play();
            }
           
            yield return StartCoroutine(WaitForAnimation(animName));

            if (!isBasicAttack && decision.Skill.SkillData.IsMelee)
            {
                _audioSource.clip = decision.Skill.SkillData.SkillSound;
                _audioSource.Play();
            }
            else
            {
                _audioSource.clip = AttackSkillSound;
                _audioSource.Play();
            }

            if (anyWillDie && !isBasicAttack)
            {
                Time.timeScale = 0.2f;
                yield return StartCoroutine(MiniGameScenesManager.Instance.Transition.MiniGameTransition());
                yield return new WaitForSecondsRealtime(1f);
                Time.timeScale = 1f;
                yield return new WaitForSeconds(0.3f);

                MiniGameScenesManager.Instance.player = targets.Find(t => t.WouldDieFromAttack(_damage)).gameObject;

                MiniGameScenesManager.Instance.StartMiniGame(_damage.Type);
                MiniGameScenesManager.Instance.Success += OnSuccess;
                MiniGameScenesManager.Instance.Fail += OnFail;
                MiniGameScenesManager.Instance.Parring += OnParrying;
            }
            else
            {
                foreach (PlayableCharacter target in targets)
                {
                    if (target == null || target.Model == null)
                        continue;

                    target.TakeDamage(_damage);
                    Vector3 position = target.Model.transform.position;

                    if (!isBasicAttack && decision?.Skill?.SkillData?.SkillPrefab != null)
                    {
                        Instantiate(decision.Skill.SkillData.SkillPrefab, position, Quaternion.identity);
                    }

                    if (FloatingTextDisplay.Instance != null)
                    {
                        FloatingTextDisplay.Instance.ShowFloatingText(position, Convert.ToInt32(_damage.Value).ToString(), FloatingTextType.Damage);
                    }
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
