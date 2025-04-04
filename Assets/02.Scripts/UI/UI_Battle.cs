using UnityEngine;

public class UI_Battle : MonoBehaviour
{
    public void GameStart()
    {
        CombatManager.Instance.InitializeCombat();
    }
}
