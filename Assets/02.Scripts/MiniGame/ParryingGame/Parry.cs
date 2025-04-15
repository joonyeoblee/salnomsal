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

        public Image Flash;
        public GameObject ParryParticles;

        [Header("난이도 설정")]
        [SerializeField]
        float _parrySpeed;
        [SerializeField] private float _parryInstatiateTime = 0;
        [SerializeField] private int _count = 3;
        [SerializeField] private int _distance = 11;

        [SerializeField] public bool GameStart = false;
        public bool IsParried = false;
        public bool AlreadyFail = false;

        public GameObject SlashEffect;
        public Animator[] ParryAnimation;
        public Animator ParryingAnimation;

        private Vector2 _successPosition;

        public bool LastParry;
        public MMF_Player CameraFeedback;
        public MMF_Player ButtonFeedback;

        public int Life;

        private int _parriedCount = 0;
        int _lastParryCount;
        bool _isChecked;
        bool AlreadySuccess;

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

        IEnumerator ParryCount(Vector2 centerPosition, float distance)
        {
            float currentTime = 0f;

            for (int i = 0; i < _count; i++)
            {
                float spawnDelay = Random.Range(0.2f, 0.8f);
                currentTime += spawnDelay;

                float angle = Random.Range(0f, Mathf.PI * 2);
                Vector2 pos = new Vector2(centerPosition.x + distance * Mathf.Cos(angle),
                    centerPosition.y + distance * Mathf.Sin(angle));
                GameObject slash = Instantiate(SlashEffect, pos, Quaternion.identity);

                SlashChecker slashCheck = slash.AddComponent<SlashChecker>();
                slashCheck.Parry = this;
                slashCheck.IsLastParry = i == _count - 1;
                slashCheck.StartTime = spawnDelay;

                GameStart = false;
                yield return new WaitForSeconds(spawnDelay);

                SlashMovement(slash);
            }
        }

        void SlashMovement(GameObject slash)
        {
            Vector2 currentPosition = slash.transform.position;
            Vector2 oppositePosition = (currentPosition - _successPosition) * -1 + _successPosition;
            float interval = slash.GetComponent<SlashChecker>().StartTime + 1;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(slash.transform.DOMove(oppositePosition, 0.1f).SetEase(Ease.OutCubic));
            sequence.AppendInterval(interval);
            sequence.AppendCallback(() => { StartCoroutine(MoveWithParticle(slash, currentPosition)); });
        }

        IEnumerator MoveWithParticle(GameObject slash, Vector2 targetPosition)
        {
            ParticleSystem ps = slash.GetComponentInChildren<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
                yield return new WaitForSeconds(ps.main.duration);
            }

            Tween moveTween = slash.transform.DOMove(targetPosition, _parrySpeed)
                .SetEase(Ease.OutCubic)
                .OnComplete(() =>
                {
                    if (!slash.GetComponent<SlashChecker>()._isParried)
                    {
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
                    parry.ParriedCheck();

                    if (parry.IsLastParry)
                    {
                        _isChecked = true;
                        Success(); // ✅ 마지막 슬래시도 카운트에 포함
                        Destroy(other.gameObject);
                    }
                    else
                    {
                        Success();
                        Destroy(other.gameObject);
                    }
                }

                foreach (Animator anim in ParryAnimation)
                {
                    anim.SetTrigger("Parry");
                }
            }
        }

        public void Fail()
        {
            if (AlreadyFail) return;

            Destroy(GameObject.FindGameObjectWithTag("Avoid"));
            AlreadyFail = true;

            Debug.Log("❌ Parry Fail");
            DOTween.KillAll();
            MiniGameScenesManager.Instance.Fail?.Invoke();
            StartCoroutine(LoadScene());
        }

        private void Success()
        {
            if (AlreadySuccess) return;

            _parriedCount++;
            Debug.Log($"🟢 Parry Success Count: {_parriedCount}/{_count}");

            if (_parriedCount >= _count)
            {
                Destroy(GameObject.FindGameObjectWithTag("Avoid"));
                AlreadySuccess = true;

                Debug.Log("✅ Parry MiniGame Success!");
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
            if (!LastParry || AlreadySuccess)
                return;

            _lastParryCount++;

            Flash.DOColor(new Color(1f, 1f, 1f, _lastParryCount / 20f), 0.1f);

            if (_lastParryCount >= 20)
            {
                Destroy(GameObject.FindGameObjectWithTag("Avoid"));
                Success(); // 중복 방지 포함됨
            }
        }
    }
}
