using UnityEngine;
using UnityEngine.SceneManagement;

namespace Jun
{
    public class MiniGameScenesManager : MonoBehaviour
    {
        public static MiniGameScenesManager instance;
        public Camera BattleSceneCamera;
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
        public void ChangeSceneToMiniGameMagic()
        {
            BattleSceneCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            BattleSceneCamera.cullingMask = LayerMask.GetMask("MiniGameUI"); // LayerA만 보임
            SceneManager.LoadScene(3, LoadSceneMode.Additive);
        }
    }
}