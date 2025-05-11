using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UI_ChestInventory : MonoBehaviour
{
    public Image[] Portraits;
    public TMP_Text[] Statas;

    public static UI_ChestInventory Instance;
    private GameObject[] _PortraitItems
    {
        get
        {
            return GameManager.Instance.Characters;
        }
    }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        // else if (Instance != this)
        // {
        //     Destroy(gameObject);
        // }
    }

    public void Initialize(PlayableCharacter character, int index)
    {
        if (character == null)
        {
            Portraits[index].gameObject.SetActive(false);
            return;
        }
        Portraits[character.Index].GetComponent<Image>().sprite = character.GetComponent<PlayableIcons>().Portrait;
    }


    
}
