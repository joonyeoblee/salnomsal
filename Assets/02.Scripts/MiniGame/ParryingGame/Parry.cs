using System.Collections;
using DG.Tweening;
using Jun;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SeongIl
{
    public class Parry : MonoBehaviour
    {
        public AudioSource ParryingSound;
        public AudioClip ParrySound;

        // flash효과
        public Image Flash;
        public GameObject ParryParticles;

        [Header("난이도 설정")]
        // 패링 타이밍 시간
        [SerializeField]
        private float _parrySpeed = 0;

        [SerializeField] private float _parryInstatiateTime = 0;
        [SerializeField] private int _count = 3;

        [SerializeField] private int _distance = 11;

        // 미니게임 시작 여부
        [SerializeField] public bool GameStart = false;

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

        public bool LastParry;
        public MMF_Player CameraFeedback;
        public MMF_Player ButtonFeedback;

        public int Life;

        // 판정 갯수세기 성공 여부 확인 위함
        private int _parriedCount = 0;
        int _lastParryCount = 0;

        bool _isChecked;

        private void Start()
        {
            _successPosition = transform.position;
            AlreadyFail = false;
            Flash.DOColor(new Color(0, 0, 0, 0), 2f).OnComplete(() => { GetComponent<ParrySequence>().GamePlay(); });

        }

        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                FinalAttack();
                StartCoroutine(Parrying());

                if (LastParry)
                {
                    ButtonFeedback.PlayFeedbacks();
                    Instantiate(ParryParticles);
                }
            }

            if (Life < 0)
            {
                Fail();
            }
        }

        public void StartGame()
        {
            StartCoroutine(ParryCount(_successPosition, _distance));
        }

        // 슬래시 소환 위치 정하기 && 갯수 정하기 
        IEnumerator ParryCount(Vector2 centerPosition, float distance)
        {
            float currentTime = 0f;

            for (int i = 0; i < _count; i++)
            {
                float spawnDelay = Random.Range(0.2f, 0.8f); // 최소 0.2f 보장
                currentTime += spawnDelay;

                // 위치 정하기
                float angle = Random.Range(0f, Mathf.PI * 2);
                Vector2 pos = new Vector2(centerPosition.x + distance * Mathf.Cos(angle),
                    centerPosition.y + distance * Mathf.Sin(angle));
                GameObject slash = Instantiate(SlashEffect, pos, Quaternion.identity);

                SlashChecker slashCheck = slash.AddComponent<SlashChecker>();
                slashCheck.Parry = this;
                if (i == _count - 1)
                {
                    slashCheck.IsLastParry = true;
                }

                slashCheck.StartTime = spawnDelay;

                // 일정 시간 후에 슬래시 작동
                GameStart = false;
                yield return new WaitForSeconds(spawnDelay);

                SlashMovement(slash);
            }
        }

        void SlashMovement(GameObject slash)
        {
            Vector2 currentPosition = slash.transform.position;
            Vector2 oppositePosition = (currentPosition - _successPosition) * -1 + _successPosition;
            float Interval = slash.GetComponent<SlashChecker>().StartTime + 1;

            Sequence sequence = DOTween.Sequence();

            // 뒤로 빠짐 (반짝 없음)
            Tween backTween = slash.transform.DOMove(oppositePosition, 0.1f).SetEase(Ease.OutCubic);
            sequence.Append(backTween);

            // 대기 시간
            sequence.AppendInterval(Interval);

            // 반짝 & 이동 시작 (돌아오기)
            sequence.AppendCallback(() => { StartCoroutine(MoveWithParticle(slash, currentPosition)); });
        }


        IEnumerator MoveWithParticle(GameObject slash, Vector2 targetPosition)
        {
            // 자식 파티클 가져오기
            ParticleSystem ps = slash.GetComponentInChildren<ParticleSystem>();
            if (ps != null)
            {
                ps.Play(); // 파티클 재생
                yield return new WaitForSeconds(ps.main.duration); // 파티클 길이만큼 대기
            }

            Tween moveTween = slash.transform.DOMove(targetPosition, _parrySpeed)
                .SetEase(Ease.OutCubic)
                .OnComplete(() => {
                    if (!slash.GetComponent<SlashChecker>()._isParried) {
                        slash.GetComponent<SlashChecker>()?.MissedCheck();
                    }
                });

            moveTween.OnUpdate(() =>
            {
                if (_isChecked)
                {
                    moveTween.Kill();
                    slash.transform.position = _successPosition;
                    LastParry = true;
                    CameraFeedback.PlayFeedbacks();
                }
            });
        }


        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Avoid") && IsParried)
            {
                ParryingSound.PlayOneShot(ParrySound);
                if (other.TryGetComponent(out SlashChecker parry))
                {
                    parry.ParriedCheck(); // 성공 체크 확정
                    if (parry.IsLastParry)
                    {
                        _isChecked = true;
                    }
                    else
                    {
                        Success();
                        Destroy(other.gameObject);
                    }
                }


                for (int i = 0; i < ParryAnimation.Length; i++)
                {
                    ParryAnimation[i].SetTrigger("Parry");
                }

            }
        }

        // 판정
        public void Fail()
        {
            if (AlreadyFail)
            {
                return;
            }

            Destroy(GameObject.FindGameObjectWithTag("Avoid"));
            AlreadyFail = true;

            Debug.Log("Fail");
            DOTween.KillAll();
            MiniGameScenesManager.Instance.Fail?.Invoke();
            StartCoroutine(LoadScene());
        }

        bool AlreadySuccess;

        private void Success()
        {
            if (AlreadySuccess) return;

            _parriedCount += 1;
            Debug.Log("Parried");

            if (_parriedCount >= _count)
            {
                Destroy(GameObject.FindGameObjectWithTag("Avoid"));

                AlreadySuccess = true;
                Debug.Log("Success");
                DOTween.KillAll();
                MiniGameScenesManager.Instance.Success?.Invoke();
                StartCoroutine(LoadScene());
            }
        }

        private IEnumerator Parrying()
        {
            IsParried = true;
            ParryingAnimation.SetTrigger("Parry");
            yield return new WaitForSeconds(0.1f);
            IsParried = false;

        }

        private IEnumerator LoadScene()
        {
            yield return new WaitForSeconds(0.2f);
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));

        }
        private void FinalAttack()
        {
            if (!LastParry)
            {
                return;
            }

            if (_lastParryCount >= 20)
            {
                Destroy(GameObject.FindGameObjectWithTag("Avoid")); // 슬래시 정리
                Success();
            }

            else
            {
                _lastParryCount += 1;
                Flash.DOColor(new Color(1f, 1f, 1f, (float)_lastParryCount / 20f), 0.1f);

            }

        }
        
        
    }
}
