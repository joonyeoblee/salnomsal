using UnityEngine;
using UnityEngine.SceneManagement;

namespace Jun
{
    public class MiniGameScenesManager : MonoBehaviour
    {
        public static MiniGameScenesManager instance;

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
        public void ChangeScene(int index)
        {
            SceneManager.LoadScene(index);
        }
        public void ChangeSceneToIndex0()
        {
            SceneManager.LoadScene(0, LoadSceneMode.Additive);
        }
    }
}