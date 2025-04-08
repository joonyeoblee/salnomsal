using TMPro;
using UnityEngine;
public class UI_Battle : MonoBehaviour
{
    public TextMeshProUGUI StatTMP;
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

    public void OnTurnStart(PlayableCharacter character)
    {
        StatTMP.text = $"HP :  {character.CurrentHealth} / {character.MaxHealth}\nCost : {character.Cost} / {character.MaxCost}\n공격력  {character.AttackPower}\n속도  {character.CurrentSpeed}\n저항력  {character.Resistance}";
    }
}
