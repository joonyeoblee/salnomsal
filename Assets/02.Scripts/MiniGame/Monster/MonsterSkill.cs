using System;
using System.Collections.Generic;
using System.Linq;
using Jun.Monster;
using UnityEngine;

namespace Jun.Skill
{
    public class SkillDecision
    {
        public Skill Skill;
        public int Index;
        public Character Target;
    }

    public class Skill
    {
        public SkillDataSO SkillData;
        public Func<Character, int> conditionalBonusPriority;
    }

    public class MonsterSkill : MonoBehaviour
    {
        public List<Skill> skills;
        public List<SkillDataSO> skillDataList;

        public void SetConditionalPriorities(List<Func<Character, int>> conditionalFuncs)
        {
            if (skills == null || skills.Count == 0)
            {
                Debug.LogError($"[{name}] SetConditionalPriorities: skills 리스트가 비어있음!");
                return;
            }

            for (int i = 0; i < conditionalFuncs.Count && i < skills.Count; i++)
            {
                skills[i].conditionalBonusPriority = conditionalFuncs[i];
                Debug.Log($"[{name}] 조건 함수 등록: Skill[{i}] → {skills[i].SkillData?.name ?? "null"}");
            }
        }

        public List<Skill> GetAvailableSkills(MonsterBase caster)
        {
            return skills
                .Where(skill => caster.Mana >= skill.SkillData.SkillCost)
                .ToList();
        }

        void Awake()
        {
            skills = new List<Skill>();
            if (skillDataList == null)
            {
                Debug.LogError($"[{name}] skillDataList null입니다.");
                return;
            }

            foreach (SkillDataSO skillData in skillDataList)
            {
                skills.Add(new Skill { SkillData = skillData });
            }
        }

        public SkillDecision ChooseSkillWithTarget(List<PlayableCharacter> players, List<PlayableCharacter> allies = null)
        {
            MonsterBase caster = GetComponent<MonsterBase>();
            List<Skill> availableSkills = GetAvailableSkills(caster);

            Skill bestSkill = null;
            Character bestTarget = null;
            int bestPriority = int.MinValue;
            int bestIndex = -1;

            for (int i = 0; i < availableSkills.Count; i++)
            {
                Skill skill = availableSkills[i];
                List<PlayableCharacter> potentialTargets = new List<PlayableCharacter>();

                switch (skill.SkillData.SkillTarget)
                {
                case TargetType.Enemy:
                    potentialTargets = players;
                    break;
                case TargetType.Ally:
                    potentialTargets = allies ?? new List<PlayableCharacter>();
                    break;
                case TargetType.None:
                    potentialTargets = new List<PlayableCharacter> { null };
                    break;
                }

                foreach (PlayableCharacter target in potentialTargets)
                {
                    if (target != null && !target.IsAlive) continue;

                    int priority = skill.SkillData.Priority;

                    if (skill.conditionalBonusPriority != null && target != null)
                    {
                        priority += skill.conditionalBonusPriority(target);
                    }

                    priority += EvaluateTargetBonus(target);

                    if (priority > bestPriority)
                    {
                        bestPriority = priority;
                        bestSkill = skill;
                        bestTarget = target;
                        bestIndex = i;
                    }
                }
            }

            return new SkillDecision
            {
                Skill = bestSkill,
                Index = bestIndex,
                Target = bestTarget
            };
        }

        int EvaluateTargetBonus(PlayableCharacter character)
        {
            if (character == null) return 0;

            int priority = 0;
            if (character.Taunt > 0) priority += 200;
            if (character.CurrentHealth < character.MaxHealth * 0.3f) priority += 10;
            if (character.DamageType == DamageType.Magic) priority += 7;
            if (character.HasBuff) priority += 5;
            if (character.IsDefending) priority -= 5;
            return priority;
        }
    }
}
