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
    }

    public class Skill
    {
        public SkillDataSO SkillData;
        public int basePriority;
        public Func<EnemyCharacter, int> conditionalBonusPriority; // 타겟 기준 추가 우선도
    }


    public class MonsterSkill : MonoBehaviour
    {
        public List<Skill> skills;

        public List<SkillDataSO> skillDataList;
        public List<int> basePriorities;

        public void SetConditionalPriorities(List<Func<EnemyCharacter, int>> conditionalFuncs)
        {
            for (int i = 0; i < conditionalFuncs.Count && i < skills.Count; i++)
            {
                skills[i].conditionalBonusPriority = conditionalFuncs[i];
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
            for (int i = 0; i < skillDataList.Count; i++)
            {
                skills.Add(new Skill
                {
                    SkillData = skillDataList[i],
                    basePriority = basePriorities[i]
                });
            }
        }

        public SkillDecision ChooseSkillWithIndex(EnemyCharacter target)
        {
            List<Skill> availableSkills = GetAvailableSkills(GetComponent<MonsterBase>());

            Skill bestSkill = null;
            int bestPriority = int.MinValue;
            int bestIndex = -1;

            for (int i = 0; i < availableSkills.Count; i++)
            {
                Skill skill = availableSkills[i];
                int priority = skill.basePriority;

                if (skill.conditionalBonusPriority != null)
                    priority += skill.conditionalBonusPriority.Invoke(target);

                if (priority > bestPriority)
                {
                    bestPriority = priority;
                    bestSkill = skill;
                    bestIndex = i;
                }
            }

            return new SkillDecision
            {
                Skill = bestSkill,
                Index = bestIndex
            };
        }
    }
}
