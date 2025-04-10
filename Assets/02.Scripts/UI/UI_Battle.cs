using TMPro;
using UnityEngine;
using Microlight.MicroBar;

public class UI_Battle : MonoBehaviour
{
    public static UI_Battle Instance;

    public TextMeshProUGUI StatTMP;
    public SpriteRenderer Portrait;
    public MicroBar HealthBar;
    public MicroBar CostBar;

    private void Awake()
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

    private void Start()
    {
        HealthBar.Initialize(0f);
        CostBar.Initialize(0f);
    }

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

    public void RefreshStatText(PlayableCharacter character)
    {
        StatTMP.text = $"HP :  {character.CurrentHealth} / {character.MaxHealth}\nCost : {character.Cost} / {character.MaxCost}\n공격력  {character.AttackPower}\n속도  {character.CurrentSpeed}\n저항력  {character.Resistance}";
    }

    public void RefreshPortrait(Sprite portrait)
    {
        Portrait.sprite = portrait;
    }

    public void RefreshHealthBar(float current, float max)
    {
        HealthBar.SetNewMaxHP(max);
        HealthBar.UpdateBar(current);
    }

    public void RefreshCostBar(float current, float max)
    {
        Debug.Log("CostBar Refresh");
        CostBar.SetNewMaxHP(max);
        CostBar.UpdateBar(current);
    }
}
