using UnityEngine;
using UnityEngine.UI;
using Microlight.MicroBar;

public class EnemyHealthIndicator : MonoBehaviour
{
    public GameObject[] Portrait;
    public MicroBar[] HealthBars;

    public void Initialize(EnemyCharacter enemy)
    {
        Portrait[enemy.Index].GetComponent<Image>().sprite = enemy.GetComponent<EnemyIcons>().Portrait;
        HealthBars[enemy.Index].Initialize(enemy.MaxHealth);
    }

    public void RefreshHealth(EnemyCharacter enemy)
    {
        HealthBars[enemy.Index].SetNewMaxHP(enemy.MaxHealth);
        HealthBars[enemy.Index].UpdateBar(enemy.CurrentHealth);
    }
}
