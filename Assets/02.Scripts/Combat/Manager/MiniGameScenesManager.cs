using System;
using SeongIl;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public enum SceneIndex
{
    Village,
    TitleScene,
    StartMapScene,
    MiniGameMagic,
    MiniGameRanged,
    MiniGameMelee
}

namespace Jun
{
    public class MiniGameScenesManager : MonoBehaviour
    {
        public static MiniGameScenesManager Instance;
        public Camera BattleSceneCamera;
        public GameObject player;
        public SceneTransition Transition;

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
            if (BattleSceneCamera.cullingMask == 0)
            {
                BattleSceneCamera.cullingMask = ~(1 << LayerMask.NameToLayer("MiniGameUI"));
            }
            else
            {

                BattleSceneCamera.cullingMask = -1;
            }
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
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            } else
            {
                Destroy(gameObject);
            }
        }

        public void ChangeToVillage()
        {
            SceneManager.LoadScene((int)SceneIndex.Village);
        }
        public void ChangeScene(SceneIndex index)
        {
            SceneManager.LoadScene((int)index);
            Transition.IsTransition?.Invoke();
        }

        public void StartMiniGame(DamageType damageType)
        {
            switch (damageType)
            {
            case DamageType.Magic:
                ChangeSceneToMiniGame(SceneIndex.MiniGameMagic);
                break;
            case DamageType.Ranged:
                ChangeSceneToMiniGame(SceneIndex.MiniGameRanged);
                break;
            case DamageType.Melee:
                ChangeSceneToMiniGame(SceneIndex.MiniGameMelee);
                break;
            }
        }

        public void ChangeSceneToMiniGame(SceneIndex index)
        {
            Debug.Log("미니게임 시작됨: 씬 인덱스 " + (int)index);
            BattleSceneCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            BattleSceneCamera.cullingMask = LayerMask.GetMask("MiniGameUI"); // MiniGameUI 레이어만 보이게
            SceneManager.LoadScene((int)index, LoadSceneMode.Additive);
            
        }
    }
}
