using UnityEngine;
using UnityEngine.SceneManagement;
public class ScenesManager : MonoBehaviour
{
    public static ScenesManager instance;

    public GameObject player;
    void Awake()
    {
        // Singleton 패턴
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeSceneToIndex0()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Additive);
    }
}
