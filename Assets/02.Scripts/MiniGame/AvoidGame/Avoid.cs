using System.Collections;
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
        // 패링 시간 
        [SerializeField]
        private float _parryTime = 0.5f;
        // 패링시스템 (패링 치기)
        public bool Parrying = false;
        // 화살 다 피함? 카운트 세기
        public int ArrowCount = 10;
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
            if (Input.GetKeyDown(KeyCode.Space) && !Parrying)
            {
                StartCoroutine(ParryingTiming());
            }

        }

        //판정 구하기
        private IEnumerator ParryingTiming()
        {
            Parrying = true;
            yield return new WaitForSeconds(_parryTime);
            Parrying = false;
        }

        // 실패   
        public void Fail()
        {
            Debug.Log("Fail");
            IsGameOver = true;
            DOTween.KillAll();
            MiniGameScenesManager.instance.Fail?.Invoke();
            Scene sceneToUnload = SceneManager.GetSceneAt(1); // 로드된 씬 중 두 번째 (0은 기본 active 씬)
            SceneManager.UnloadSceneAsync(sceneToUnload);
            
        }
        // 성공
        public void Success()
        {
            Debug.Log("Success");
            IsGameOver = true;
            DOTween.KillAll();
            MiniGameScenesManager.instance.Success?.Invoke();
            Scene sceneToUnload = SceneManager.GetSceneAt(1); // 로드된 씬 중 두 번째 (0은 기본 active 씬)
            SceneManager.UnloadSceneAsync(sceneToUnload);
        }

        // 패링 성공
        public void ParryingSuccess()
        {
            ParrySound.PlayOneShot(ParrySound.clip);
            Debug.Log("ParryingSuccess");
            IsGameOver = true;
            DOTween.KillAll();
            MiniGameScenesManager.instance.Parring?.Invoke();
            Scene sceneToUnload = SceneManager.GetSceneAt(1); // 로드된 씬 중 두 번째 (0은 기본 active 씬)
            SceneManager.UnloadSceneAsync(sceneToUnload);
        }
    }
}
