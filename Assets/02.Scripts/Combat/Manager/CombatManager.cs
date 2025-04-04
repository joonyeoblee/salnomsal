using System.Collections.Generic;
using UnityEngine;

namespace Jun{
    [DefaultExecutionOrder(-1)]
    public class CombatManager : MonoBehaviour
    {
        public static CombatManager Instance;
        public List<EnemyCharacter> Monsters;
        public List<PlayableCharacter> PlayableCharacter;
        public PlayableCharacter CurrentActor;
        public SkillSlot SelectedSkill;
        public List<ITargetable> Target;
        public int SpeedIncrementPerTurn;

        public List<ITurnActor> TurnOrder = new List<ITurnActor>();
        
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void InitializeCombat()
        {

        }

        public void SetOrder()
        {
            TurnOrder.Sort((a, b) => b.CurrentSpeed.CompareTo(a.CurrentSpeed));
        }

        public void SetSelectedSkill(SkillSlot slot)
        {
            Target.Clear();
            SelectedSkill = slot;
        }

        public void GetTarget(ITargetable target)
        {
            if (SelectedSkill == SkillSlot.None)
            {
                return;
            }

            SkillDataSO selectedSkillData = CurrentActor.Skills[(int)SelectedSkill];
            if (selectedSkillData.SkillRange == SkillRange.Single)
            {
                Target.Add(target);
                Debug.Log("단일 타겟 스킬");
            }
            else if (selectedSkillData.SkillRange == SkillRange.Global)
            {
                Debug.Log("전체 타겟 스킬");
                if (selectedSkillData.SkillTarget == SkillTarget.Ally)
                {
                    foreach (ITargetable ally in PlayableCharacter)
                    {
                        if (ally.IsAlive)
                        {
                            Target.Add(ally);
                        }
                    }
                }
                else if (selectedSkillData.SkillTarget == SkillTarget.Enemy)
                {
                    foreach (ITargetable enemy in Monsters)
                    {
                        if (enemy.IsAlive)
                        {
                            Target.Add(enemy);
                        }
                    }
                }
            }
        }

        public void StartTurn()
        {
            ITurnActor unit = TurnOrder[0];
            unit.StartTurn();
            // UI에 CurrentCharacter에 대한 정보 표시 추가
        }

        public void EndTurn(ITurnActor unit)
        {
            unit.CurrentSpeed = unit.BasicSpeed;
            foreach (ITurnActor turnActor in TurnOrder)
            {
                turnActor.CurrentSpeed += SpeedIncrementPerTurn;
            }
            TurnOrder.Add(unit);
        }

        public void asdasd()
        {
            Monsters[0].StartTurn();
        }
    }

}
