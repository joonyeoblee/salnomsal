using System.Collections;
using DG.Tweening;
using Jun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SeongIl
{
    public class Parry : MonoBehaviour
    {
        // flash효과
        public Image Flash;
        
        [Header("난이도 설정")]
        // 패링 타이밍 시간
        [SerializeField]
        private float _parrySpeed = 0;
        
        [SerializeField]
        private float _parryInstatiateTime = 0;
        
        [SerializeField]
        private int _count = 3;

        [SerializeField]
        private int _distance = 11;
        
        // 미니게임 시작 여부
        [SerializeField]
        public bool GameStart = false;
        // 패링 중임? 
        public bool IsParried = false;
        // 실패함?
        public bool AlreadyFail = false;
        
        // 미니게임 이펙트
        public GameObject SlashEffect;
        // 애니메이션
        public Animator[] ParryAnimation;
        public Animator ParryingAnimation;
        // 패링 판단 여부 위치
        private Vector2 _successPosition;
        
        // 판정 갯수세기 성공 여부 확인 위함
        private int _parriedCount = 0;
        private void Start()
        {
            _successPosition = transform.position;
            AlreadyFail = false; 
            Flash.DOColor(new Color(0, 0, 0, 0), 2f).OnComplete(() =>
            {
                GetComponent<ParrySequence>().GamePlay();
            });
   
        }

        private void Update()
        {
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                
                StartCoroutine(Parrying());
                
            }
            if (!GameStart)
            {
                return;
            }
            
            StartCoroutine(ParryCount(_successPosition, _distance));


        }

        // 슬래시 움직임 버전 2
        private void SlashMovement(GameObject slash)
        {
            Vector2 currentPosition = slash.transform.position; 
            Vector2 oppositePosition = (currentPosition - _successPosition) * -1 + _successPosition;
            Sequence sequence = DOTween.Sequence();
            sequence.Append(slash.transform.DOMove(oppositePosition, 0.1f).SetEase(Ease.OutCubic));
     
            
            sequence.AppendInterval(      + 1);
            
            sequence.Append(slash.transform.DOMove(currentPosition, _parrySpeed).SetEase(Ease.OutCubic).OnStart(() =>
            {
                StartCoroutine(FlashBackGround());
            }).OnComplete(() =>
            {
                
                slash.GetComponent<SlashChecker>()?.MissedCheck();
            }));        

            
        }
        
        // 슬래시 소환 위치 정하기 && 갯수 정하기 
        private IEnumerator ParryCount(Vector2 centerPosition, float distance)
        {
            for (int i = 0; i < _count; i++) // 예시 값
            {
                float spawnTime = Random.Range(0.2f, 1.2f); 
                // 위치 정하기
                float angle = Random.Range(0f, Mathf.PI *2);
                Vector2 pos =  new Vector2( centerPosition.x + distance * Mathf.Cos(angle), centerPosition.y + distance * Mathf.Sin(angle));
                GameObject slash = Instantiate(SlashEffect,pos, Quaternion.identity); 
                SlashChecker slashCheck = slash.AddComponent<SlashChecker>();
                slashCheck.StartTime = spawnTime;
                slashCheck.OnMissed = Fail;
                // 움직임 시작
                SlashMovement(slash);
                GameStart = false;
                yield return new WaitForSeconds(spawnTime);
            }

        }
        
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Avoid") && IsParried)
            {
              Success();   
              Destroy(other.gameObject);
              for (int i = 0; i < ParryAnimation.Length; i++)
              {
                  ParryAnimation[i].SetTrigger("Parry");
              }
              
            }
        }

        // 판정
        private void Fail()
        {
            if (AlreadyFail)
            {
                return;
            }
            
            AlreadyFail = true;
            
            Debug.Log("Fail");
            MiniGameScenesManager.instance.Fail?.Invoke();
            Scene sceneToUnload = SceneManager.GetSceneAt(1); // 로드된 씬 중 두 번째 (0은 기본 active 씬)
            SceneManager.UnloadSceneAsync(sceneToUnload);
        }

        bool AlreadySuccess;

        private void Success()
        {
            if (AlreadySuccess) return;

            _parriedCount += 1;
            Debug.Log("Parried");

            if (_parriedCount >= _count)
            {
                AlreadySuccess = true;
                Debug.Log("Success");
                MiniGameScenesManager.instance.Success?.Invoke();
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));
            }
        }
        private IEnumerator FlashBackGround()
        {
            yield return new WaitForSeconds(0.25f);
            Flash.DOColor(new Color(1, 1, 1, 0.2f), 0.2f).OnComplete(() =>
            {
                Flash.DOColor(new Color(1, 1, 1, 0f), 0.2f);
            });
        }

        private IEnumerator Parrying()
        {
            IsParried = true;
            ParryingAnimation.SetTrigger("Parry");
            yield return new WaitForSeconds(0.1f);
            IsParried = false;
            
        }
    }
}
