using System.Collections;
using DG.Tweening;
using Jun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SeongIl
{
    public class Avoid : MonoBehaviour
    {
        private AvoidSpawner Spawner;
        //  parrying 사운드
        public AudioSource ParrySound;

        // 화살 다 피함? 카운트 세기
        public int ArrowCount;
        // 스폰 속도
        public float MinTime;
        public float MaxTime;
        public int SuccessCount;
        // 목숨
        public int Lives = 3;
        // 게임 시작
        public bool IsGameOver = true;
        public SpriteRenderer Icon;

        void Start()
        {
            // int index = MiniGameScenesManager.Instance.player.GetComponent<PlayableCharacter>().Index;
            // Icon.sprite = GameManager.Instance.PortraitItems[index].portrait.Icon;
            Spawner = GetComponent<AvoidSpawner>();
        }

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
            
            DOTween.KillAll();
            MiniGameScenesManager.Instance.Fail?.Invoke();
            StartCoroutine(LoadScene());

        }

        // 성공
        public void Success()
        {
            Debug.Log("Success");
            GameStop();
            
            DOTween.KillAll();
            MiniGameScenesManager.Instance.Success?.Invoke();
            StartCoroutine(LoadScene());
        }

        // 패링 성공
        public void ParryingSuccess()
        {
            ParrySound.PlayOneShot(ParrySound.clip);
            
            DOTween.KillAll();
            MiniGameScenesManager.Instance.Parring?.Invoke();
            StartCoroutine(LoadScene());
        }

        // 게임 종료하기
        public void GameStop()
        {
            IsGameOver = true;
            Spawner?.StopAllCoroutines();
        }
        //게임 시작
        public void GameStart()
        {
            Spawner.SpawnStart(ArrowCount,this);
            IsGameOver = false;
        }
        
        private IEnumerator LoadScene()
        {
            yield return new WaitForSeconds(0.2f);
            Scene sceneToUnload = SceneManager.GetSceneAt(1); // 로드된 씬 중 두 번째 (0은 기본 active 씬)
            SceneManager.UnloadSceneAsync(sceneToUnload);
        }
    }
}
