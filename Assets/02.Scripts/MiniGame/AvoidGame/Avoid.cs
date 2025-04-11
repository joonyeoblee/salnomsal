using System.Collections;
using Com.LuisPedroFonseca.ProCamera2D.TopDownShooter;
using DG.Tweening;
using Jun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SeongIl
{
    public class Avoid : MonoBehaviour
    {
        //  parrying 사운드
        public AudioSource ParrySound;

        // 화살 다 피함? 카운트 세기
        public int ArrowCount;

        // 스폰 속도
        public float MinTime;

        public float MaxTime;

        // 게임 시작
        public bool IsGameOver = false;
        public int SuccessCount = 0;


        // 피하기 시작하기
        private void Update()
        {
            if (IsGameOver)
            {
                return;
            }

            // 게임 끝내기
            if (SuccessCount >= ArrowCount)
            {
                Success();
            }

        }

        // 실패   
        public void Fail()
        {
            Debug.Log("Fail");
            GameStop();
            IsGameOver = true;
            DOTween.KillAll();
            MiniGameScenesManager.Instance.Fail?.Invoke();
            Scene sceneToUnload = SceneManager.GetSceneAt(1); // 로드된 씬 중 두 번째 (0은 기본 active 씬)
            SceneManager.UnloadSceneAsync(sceneToUnload);

        }

        // 성공
        public void Success()
        {
            Debug.Log("Success");
            IsGameOver = true;
            DOTween.KillAll();
            MiniGameScenesManager.Instance.Success?.Invoke();
            Scene sceneToUnload = SceneManager.GetSceneAt(1); // 로드된 씬 중 두 번째 (0은 기본 active 씬)
            SceneManager.UnloadSceneAsync(sceneToUnload);
        }

        // 패링 성공
        public void ParryingSuccess()
        {
            ParrySound.PlayOneShot(ParrySound.clip);
            IsGameOver = true;
            
            DOTween.KillAll();
            MiniGameScenesManager.Instance.Parring?.Invoke();
            Scene sceneToUnload = SceneManager.GetSceneAt(1); // 로드된 씬 중 두 번째 (0은 기본 active 씬)
            SceneManager.UnloadSceneAsync(sceneToUnload);
        }

        // 게임 종료하기
        private void GameStop()
        {
            AvoidSpawner spawner = GetComponent<AvoidSpawner>();
            spawner?.StopAllCoroutines();
        }
    }
}
