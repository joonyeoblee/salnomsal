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
        public Func<Character, int> conditionalBonusPriority; // 타겟 기준 추가 우선도
    }


    public class MonsterSkill : MonoBehaviour
    {
        public List<Skill> skills;

        public SkillDataSO Skill1;

        public SkillDataSO Skill2;
        void Start()
        {
            skills = new List<Skill>
            {
                new Skill
                {
                    SkillData = Skill1,
                    basePriority = 8,
                    conditionalBonusPriority = target =>
                        target.CurrentHealth < target.MaxHealth * 0.3f ? 5 : 0 // 체력이 30% 이하일 때 우선도 +5
                },
                new Skill
                {
                    SkillData = Skill2,
                    basePriority = 6,
                    conditionalBonusPriority = target =>
                        target.IsDefending ? 10 : 0 // 방어 상태일 때 우선도 +10
                }
            };
        }
        List<Skill> GetAvailableSkills(MonsterBase caster)
        {
            return skills
                .Where(skill => caster.Mana >= skill.SkillData.SkillCost)
                .ToList();
        }

        public SkillDecision ChooseSkillWithIndex(Character target)
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
                {
                    priority += skill.conditionalBonusPriority.Invoke(target);

                }

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
