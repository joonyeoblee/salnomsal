using UnityEngine;
using TMPro;
using Microlight.MicroBar;
using UnityEngine.UI;
using Equipment;
using Unity.VisualScripting;
using JetBrains.Annotations;

public class UI_InBattle : MonoBehaviour
{
    public GameObject Portrait;
    public TextMeshProUGUI Info;
    public MicroBar HealthBar;
    public MicroBar CostBar;
    public Button DefaultAttack;
    public Button Skill1;
    public Button Skill2;

    public void Initialize(PlayableCharacter character)
    {
        PlayableIcons icons = character.GetComponent<PlayableIcons>();

        if (icons != null)
        {
            Portrait.GetComponent<Image>().sprite = icons.Portrait;
            DefaultAttack.GetComponent<Image>().sprite = icons.DefaultAttackIcon;
            Skill1.GetComponent<Image>().sprite = icons.Skill1Icon;
            Skill2.GetComponent<Image>().sprite = icons.Skill2Icon;
        }


        HealthBar.Initialize(character.MaxHealth);
        CostBar.Initialize(character.MaxCost);
        Refresh(character);
    }

    public void Refresh(PlayableCharacter character)
    {
        Info.text = $"HP :  {character.CurrentHealth} / {character.MaxHealth}\nCost : {character.Cost} / {character.MaxCost}\n공격력 : {character.AttackPower}\n속도 : {character.CurrentSpeed}\n저항력 : {character.Resistance}";
        HealthBar.SetNewMaxHP(character.MaxHealth);
        HealthBar.UpdateBar(character.CurrentHealth);
        CostBar.SetNewMaxHP(character.MaxCost);
        CostBar.UpdateBar(character.Cost);
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
