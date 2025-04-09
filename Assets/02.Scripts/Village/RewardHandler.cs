using UnityEngine;

public class RewardHandler : MonoBehaviour
{
    public GameObject Portal;

    void Start()
    {
        if (GameManager.Instance.BossKill)
        {
            Debug.Log("보상 지급");
            Portal.GetComponent<SpawnPlayer>().Spawn();
            GameManager.Instance.ResetBossKill();
        }
    }
}
