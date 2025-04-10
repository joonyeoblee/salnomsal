using UnityEngine;

public class ResultHandler : MonoBehaviour
{
    public GameObject Portal;

    private GameManager _gameManager;

    void Start()
    {
        _gameManager = GameManager.Instance;

        if (_gameManager.BossKill)
        {
            Debug.Log("보상 지급");
            Portal.GetComponent<SpawnPlayer>().Spawn();
            GameManager.Instance.ResetBossKill();
        }
        
    }
}
