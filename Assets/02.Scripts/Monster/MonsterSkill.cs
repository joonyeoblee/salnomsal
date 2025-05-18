using System;
using System.Collections.Generic;
using System.Linq;
using Jun.Monster;
using UnityEngine;

namespace Jun.Skill
{
    public enum ConditionType
    {
        LowHealth, // 체력이 낮을 때
        HasBuff, // 버프 있을 때
        IsDefending, // 방어 중일 때
        HealthBelowX, // 체력이 X 이하면
        HasImmune
    }


    [Serializable]
    public class SkillCondition
    {
        public ConditionType conditionType;
        public int threshold; // 예: HealthBelowX일 때 사용
        public int bonusScore;
    }

    [Serializable]
    public class SkillConditionGroup
    {
        public List<SkillCondition> conditions;
    }

    public class SkillDecision
    {
        public Skill Skill;
        public int Index;
    }

    public class Skill
    {
        public SkillDataSO SkillData;
        
        public List<Func<Character, int>> conditionalBonusPriorities = new List<Func<Character, int>>();
    }

    
    public class MonsterSkill : MonoBehaviour
    {
        public List<Skill> skills;
        public List<SkillDataSO> skillDataList;
        public void SetConditionalPriorities(List<List<Func<Character, int>>> conditionalFuncGroups)
        {
            if (skills == null || skills.Count == 0)
            {
                Debug.LogError($"[{name}] SetConditionalPriorities: skills 리스트가 비어있음!");
                return;
            }

            for (var i = 0; i < conditionalFuncGroups.Count && i < skills.Count; i++)
            {
                skills[i].conditionalBonusPriorities = conditionalFuncGroups[i];

                var skillName = skills[i].SkillData?.name ?? "null";
                Debug.Log($"[{name}] 조건 함수 그룹 등록: Skill[{i}] → {skillName}, 조건 수: {conditionalFuncGroups[i].Count}");
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
                    SkillData = skillDataList[i]
                });
            }
        }

        public SkillDecision ChooseSkillWithIndex(Character target)
        {
            MonsterBase caster = GetComponent<MonsterBase>();
            List<Skill> availableSkills = GetAvailableSkills(caster);

            // 사용 가능한 스킬이 없으면 기본 공격을 리턴
            if (availableSkills.Count == 0)
            {
                Skill basicAttack = skills.FirstOrDefault(s => s.SkillData.SkillCost == 0);
                if (basicAttack != null)
                {
                    Debug.Log($"[{name}] 마나 부족으로 기본 공격 선택: {basicAttack.SkillData.name}");
                    return new SkillDecision
                    {
                        Skill = basicAttack,
                        Index = skills.IndexOf(basicAttack)
                    };
                }

                Debug.LogWarning($"[{name}] 마나 부족 + 기본 공격 없음");
                return new SkillDecision { Skill = null, Index = -1 };
            }

            Skill bestSkill = null;
            int bestPriority = int.MinValue;
            int bestIndex = -1;

            for (int i = 0; i < availableSkills.Count; i++)
            {
                Skill skill = availableSkills[i];
                int priority = skill.SkillData.Priority;

                // ✅ 조건 함수들 모두 평가해서 합산
                if (skill.conditionalBonusPriorities != null && skill.conditionalBonusPriorities.Count > 0)
                {
                    var bonus = skill.conditionalBonusPriorities.Sum(func => func(target));
                    priority += bonus;
                    Debug.Log($"[{name}] {skill.SkillData.name}: base {skill.SkillData.Priority}, bonus {bonus} → total {priority}");
                }
                else
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
            }
            else
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
