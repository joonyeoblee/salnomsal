using UnityEngine;

public class UI_Battle : MonoBehaviour
{
    public void OnClickBattleStart()
    {
        CombatManager.Instance.InitializeCombat();
    }

    public void OnClickDefaultAttack()
    {
        CombatManager.Instance.SetSelectedSkill(SkillSlot.DefaultAttack);
    }

    public void OnClickSkill1()
    {
        CombatManager.Instance.SetSelectedSkill(SkillSlot.Skill1);
    }

    public void OnClickSkill2()
    {
        CombatManager.Instance.SetSelectedSkill(SkillSlot.Skill2);
    }
}
