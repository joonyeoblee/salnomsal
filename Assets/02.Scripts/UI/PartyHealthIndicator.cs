using Microlight.MicroBar;
using UnityEngine;
using UnityEngine.UI;
public class PartyHealthIndicator : MonoBehaviour
{
    public GameObject[] Portrait;
    public MicroBar[] HealthBars;

    public void Initialize(PlayableCharacter character, int index)
    {
        if (character == null)
        {
            Portrait[index].SetActive(false);
            HealthBars[index].gameObject.SetActive(false);
            return;
        }
        Portrait[character.Index].GetComponent<Image>().sprite = character.GetComponent<PlayableIcons>().Portrait;
        HealthBars[character.Index].Initialize(character.MaxHealth);
    }

    public void RefreshHealth(PlayableCharacter character)
    {
        Debug.Log("Refresh Playable Indicator");
        HealthBars[character.Index].SetNewMaxHP(character.MaxHealth);
        HealthBars[character.Index].UpdateBar(character.CurrentHealth);
    }
}
