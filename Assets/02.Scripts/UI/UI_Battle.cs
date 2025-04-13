using TMPro;
using UnityEngine;
using Microlight.MicroBar;

public class UI_Battle : MonoBehaviour
{
    public static UI_Battle Instance;

    public UI_InBattle[] BattleUI;
    public PartyHealthIndicator PartyHealthIndicator;
    public EnemyHealthIndicator EnemyHealthIndicator;

    
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

    public void SwitchUI(int index)
    {
        HideBattleUI();
        BattleUI[index].gameObject.SetActive(true);
    }

    public void HideBattleUI()
    {
        foreach (UI_InBattle ui in BattleUI)
        {
            ui.gameObject.SetActive(false);
        }
    }

    public void ShowPartyHealthIndicator()
    {
        PartyHealthIndicator.gameObject.SetActive(true);
    }

    public void HidePartyHealthIndicator()
    {
        PartyHealthIndicator.gameObject.SetActive(false);
    }

    public void ShowEnemyHealthIndicator()
    {
        EnemyHealthIndicator.gameObject.SetActive(true);
    }

    public void HideEnemyHealthIndicator()
    {
        EnemyHealthIndicator.gameObject.SetActive(false);
    }
}
