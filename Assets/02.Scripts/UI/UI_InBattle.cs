using UnityEngine;
using TMPro;
using Microlight.MicroBar;
using UnityEngine.UI;
using Equipment;
using Unity.VisualScripting;
using JetBrains.Annotations;
using DG.Tweening;

public class UI_InBattle : MonoBehaviour
{
    public GameObject Portrait;
    public TextMeshProUGUI Info;
    public MicroBar HealthBar;
    public MicroBar CostBar;
    public Button DefaultAttack;
    public Button Skill1;
    public Button Skill2;


    public void OnEnable()
    {
        ResetButton();
    }

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
        ResetButton();
    }

    public void Refresh(PlayableCharacter character)
    {
        Info.text = $"HP :  {(int)character.CurrentHealth} / {(int)character.MaxHealth}\nCost : {character.Cost} / {character.MaxCost}\n공격력 : {(int)character.AttackPower}\n속도 : {character.CurrentSpeed}\n저항력 : {character.Resistance}";
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

    public void OnClickTween(Button button)
    {
        ResetButton();

        Transform buttonTransform = button.transform;

        buttonTransform.DOScale(1.1f, 0.1f).SetEase(Ease.OutQuad).OnComplete(() => {});
        Debug.Log(button.name + " clicked");
    }

    public void ResetButton()
    {
        DefaultAttack.transform.DOKill();
        Skill1.transform.DOKill();
        Skill2.transform.DOKill();

        DefaultAttack.transform.localScale = Vector3.one;
        Skill1.transform.localScale = Vector3.one;
        Skill2.transform.localScale = Vector3.one;
        Debug.Log("ResetButton");
    }
}
