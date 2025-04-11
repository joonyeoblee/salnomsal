using UnityEngine;
using UnityEngine.UI;
using Microlight.MicroBar;
using System.Collections.Generic;

public class EnemyHealthIndicator : MonoBehaviour
{
    public GameObject[] Portrait;
    public MicroBar[] HealthBars;

    /*
    public void Initialize(EnemyCharacter enemy)
    {
        Portrait[enemy.Index].GetComponent<Image>().sprite = enemy.GetComponent<EnemyIcons>().Portrait;
        HealthBars[enemy.Index].Initialize(enemy.MaxHealth);
    }
    */

    public void Initialize(List<EnemyCharacter> enemies)
    {
        // 전체 꺼두기
        for (int i = 0; i < Portrait.Length; i++)
        {
            Portrait[i].SetActive(false);
            HealthBars[i].gameObject.SetActive(false);
        }

        // 오른쪽부터 채우기
        int offset = Portrait.Length - enemies.Count;
        for (int i = 0; i < enemies.Count; i++)
        {
            int slotIndex = offset + i;

            Portrait[slotIndex].SetActive(true);
            Portrait[slotIndex].GetComponent<Image>().sprite = enemies[i].GetComponent<EnemyIcons>().Portrait;

            HealthBars[slotIndex].gameObject.SetActive(true);
            HealthBars[slotIndex].Initialize(enemies[i].MaxHealth);

            // EnemyCharacter가 자신의 UI 인덱스를 알 수 있도록 해두면 편해짐
            enemies[i].Index = slotIndex;
        }
    }

    public void RefreshHealth(EnemyCharacter enemy)
    {
        Debug.Log("Refresh Enemy Indicator");
        HealthBars[enemy.Index].SetNewMaxHP(enemy.MaxHealth);
        HealthBars[enemy.Index].UpdateBar(enemy.CurrentHealth);
    }
}
