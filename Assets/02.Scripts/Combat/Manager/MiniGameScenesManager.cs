using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

namespace Jun
{
    public class MiniGameScenesManager : MonoBehaviour
    {
        public static MiniGameScenesManager instance;
        public Camera BattleSceneCamera;
        public GameObject player;

        public Action Success;
        public Action Fail;
        public Action Parring;

        void OnEnable()
        {
            Success += ChangeCamera;
            Success += LogSuccess;

            Fail += ChangeCamera;
            Fail += LogFail;

            Parring += ChangeCamera;
            Parring += LogParring;
        }

        void OnDisable()
        {
            Success -= ChangeCamera;
            Success -= LogSuccess;

            Fail -= ChangeCamera;
            Fail -= LogFail;

            Parring -= ChangeCamera;
            Parring -= LogParring;
        }

        void ChangeCamera()
        {
            BattleSceneCamera.cullingMask = ~(1 << LayerMask.NameToLayer("MiniGameUI"));
        }

        void LogSuccess()
        {
            Debug.Log("[MiniGame] 성공 이벤트 발생");
        }

        void LogFail()
        {
            Debug.Log("[MiniGame] 실패 이벤트 발생");
        }

        void LogParring()
        {
            Debug.Log("[MiniGame] 패링 이벤트 발생");
        }

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

        public void StartMiniGame(DamageType damageType)
        {
            switch (damageType)
            {
            case DamageType.Magic:
                ChangeSceneToMiniGame(3);
                break;
            case DamageType.Ranged:
                ChangeSceneToMiniGame(4);
                break;
            case DamageType.Melee:
                ChangeSceneToMiniGame(5);
                break;
            }
        }

        public void ChangeSceneToMiniGame(int index)
        {
            Debug.Log("미니게임 시작됨: 씬 인덱스 " + index);
            BattleSceneCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            BattleSceneCamera.cullingMask = LayerMask.GetMask("MiniGameUI"); // MiniGameUI 레이어만 보이게
            SceneManager.LoadScene(index, LoadSceneMode.Additive);
        }
    }
}
