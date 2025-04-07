using System.Collections.Generic;
using Team;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<TeamSlot> TeamSlots = new List<TeamSlot>(3);
    public List<PlayableCharacter> Characters = new List<PlayableCharacter>(3);
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void Expedition()
    {

    }
}
