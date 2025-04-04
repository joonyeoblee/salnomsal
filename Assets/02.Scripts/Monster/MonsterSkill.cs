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

        public Func<Character, int> conditionalBonusPriority; // 타겟 기준 추가 우선도
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
            var available = skills
                .Where(skill => caster.Mana >= skill.SkillData.SkillCost)
                .ToList();

            Debug.Log($"[{name}] 사용 가능한 스킬 개수: {available.Count}");
            foreach (Skill skill in available)
            {
                Debug.Log($"[{name}] 사용 가능 스킬: {skill.SkillData.name}, 코스트: {skill.SkillData.SkillCost}, 현재 마나: {caster.Mana}");
            }

            return available;
        }

        void Awake()
        {
            skills = new List<Skill>();

            if (skillDataList == null)
            {
                Debug.LogError($"[{name}] skillDataList null입니다.");
                return;
            }
            

            for (int i = 0; i < skillDataList.Count; i++)
            {
                skills.Add(new Skill
                {
                    SkillData = skillDataList[i],
                    
                });

                //Debug.Log($"[{name}] 스킬 초기화됨: {skillDataList[i].name}, 우선도: {basePriorities[i]}");
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public SkillDecision ChooseSkillWithIndex(Character target)
        {
            MonsterBase caster = GetComponent<MonsterBase>();
            List<Skill> availableSkills = GetAvailableSkills(caster);

            Skill bestSkill = null;
            int bestPriority = int.MinValue;
            int bestIndex = -1;

            for (int i = 0; i < availableSkills.Count; i++)
            {
                Skill skill = availableSkills[i];
                int priority = skill.SkillData.Priority;

                if (skill.conditionalBonusPriority != null)
                {
                    int bonus = skill.conditionalBonusPriority.Invoke(target);
                    priority += bonus;
                    Debug.Log($"[{name}] {skill.SkillData.name}: base {skill.SkillData.Priority}, bonus {bonus} → total {priority}");
                } else
                {
                    Debug.Log($"[{name}] {skill.SkillData.name}: base {priority} (조건 함수 없음)");
                }

                if (priority > bestPriority)
                {
                    bestPriority = priority;
                    bestSkill = skill;
                    bestIndex = i;
                }
            }

            if (bestSkill != null)
            {
                Debug.Log($"[{name}] 최종 선택 스킬: {bestSkill.SkillData.name}, 우선도: {bestPriority}");
            } else
            {
                Debug.LogWarning($"[{name}] 선택된 스킬이 없음 (null)");
            }

            return new SkillDecision
            {
                Skill = bestSkill,
                Index = bestIndex
            };
        }
    }
}
