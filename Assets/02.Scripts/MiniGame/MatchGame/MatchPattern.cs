using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Jun;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

namespace SeongIl
{
    public class MatchPattern : MonoBehaviour
    {
        public Image Book;
        public TextMeshProUGUI Text;
        private string _currentkey;

        public Image Flash;
        
        // 이펙트
        public Animator MagicEffect;
        public GameObject[] MagicVFX;
        
        public Image[] MagicCircle;
        
        
        
        [Header("시간 설정")]
        public float TimeLimit = 0;
        public float BonusDecline;
        public float IncreaseTime;
        public float DeclineTime;
        
        [Header("게임 시작")]
        public bool _isGameActive = true;
        
        private Queue<string> _keyQueue = new Queue<string>();

        public int KeyCount = 5;

        private void Start()
        {
            /*// 난이도 조절하기
            TimeLimit =  // 차오르는 속도
            BonusDecline = // 전부 다 쳤을 때 fillAmount 감소
            IncreaseTime = // 틀렸을 때 차오르는 양
            DeclineTime = // 성공했을 때 줄어드는 양
            */
            
        }

        private void Update()
        {
            
            if (!_isGameActive)
            {
                return;
            }
            
            MagicCircle[0].fillAmount += TimeLimit * Time.deltaTime;
            
            if (MagicCircle[0].fillAmount >= 1)
            {
                Fail();
            }
            else if (MagicCircle[0].fillAmount <= 0.007f)
            {
                Success();
            }

            if (Input.anyKeyDown)
            {
                if (Input.GetKeyDown(_keyQueue.Peek().ToLower()))
                {
                    _keyQueue.Dequeue();
                    if (_keyQueue.Count > 0)
                    {
                        DisplayKeys();

                        MagicCircle[0].fillAmount -= DeclineTime;
                        DecreaseFill(DeclineTime, 0.1f);
                        StartCoroutine(ShakeObjetct(MagicCircle[0], new Vector2(0, 0), 3, -0.05f, 0.05f, 0.1f));
                        ShakeBook(3, 0.05f, -3, 3);

                    }
                    else
                    {
                        Debug.Log("Reload");
                        GenerateKeysQueue();
                        DisplayKeys();
                        MagicCircle[0].fillAmount -= BonusDecline;
                    }

                    MagicEffect.SetTrigger("Key");
                    if (MagicVFX[0].activeInHierarchy)
                    {
                        MagicVFX[1].SetActive(true);

                    }
                    else
                    {   
                        MagicVFX[0].SetActive(true);

                    }
                }
                
                else
                {
                    FailCircle();
                    MagicCircle[0].fillAmount += IncreaseTime;
                }
            }
        }
        private void GenerateKeysQueue()
        {
            string[] keys = { "Q", "W", "E", "R", "T" };
            _keyQueue.Clear();

            for (int i = 0; i < KeyCount; i++)
            {
                int index = Random.Range(0, keys.Length);
                _keyQueue.Enqueue(keys[index]);
            }
            
        }

        private void DisplayKeys()
        {
            Text.text = string.Join(" ", _keyQueue.ToArray());
            if (Text == null)
            {
                Debug.Log("널이에유");
            }
            Debug.Log(string.Join(" ", _keyQueue.ToArray()));
        }
        
        
        public void Fail()
        {
            Debug.Log("Fail");
            _isGameActive = false;
            DOTween.KillAll();
            MiniGameScenesManager.instance.Fail?.Invoke();
            Scene sceneToUnload = SceneManager.GetSceneAt(1); // 로드된 씬 중 두 번째 (0은 기본 active 씬)
            SceneManager.UnloadSceneAsync(sceneToUnload);
            
        }

        public void Success()
        {
            Debug.Log("성공");
            _isGameActive = false;
            DOTween.KillAll();
            MiniGameScenesManager.instance.Success?.Invoke();
            Scene sceneToUnload = SceneManager.GetSceneAt(1); // 로드된 씬 중 두 번째 (0은 기본 active 씬)
            SceneManager.UnloadSceneAsync(sceneToUnload);
        }
        
        // 겜시작
        public IEnumerator GameStart()
        {
            yield return new WaitForSeconds(4f);
            GenerateKeysQueue();
            DisplayKeys();
            MagicCircle[0].fillAmount = 0.1f;
            _isGameActive = true;
        }
        
        // 마법진 흔들기
        private IEnumerator ShakeObjetct(Image circle, Vector2 origin, int count, float min, float max, float duration)
        {
            for (int i = 0; i < count; i++)
            {
                float randX = Random.Range(min , max);
                float randY = Random.Range(min, max);
                circle.rectTransform.DOMove(new Vector3(origin.x + randX ,origin.y +randY, 0), duration);
                yield return new WaitForSeconds(duration);
                
            }
            
            circle.rectTransform.anchoredPosition = origin;
        }
        
        // 책 흔들기
        private void ShakeBook(int count, float duration, int min, int max)
        {
            Book.transform.DOKill();
            Sequence sequence = DOTween.Sequence();

            for (int i = 0; i < count; i++)
            {
                float angle = Random.Range(min, max);
                sequence.Append(Book.rectTransform.DORotate(new Vector3(0 , 0, angle), duration));
            }
            sequence.Append(Book.rectTransform.DORotate(new Vector3(0 , 0 , 0), duration));
        }

        // 실패시 호출
        private void FailCircle()
        {
            MagicCircle[2].DOKill();
            MagicCircle[2].gameObject.SetActive(true); 
            Sequence sequence = DOTween.Sequence();
            StartCoroutine(ShakeObjetct(MagicCircle[2], Vector2.zero, 3, -0.1f, 0.1f, 0.2f));
            Flash.DOColor(new Color(1, 1, 1, 0.1f ), 0.4f).OnComplete((() =>
            {
                Flash.DOColor(new Color(1,1,1, 0),1f).SetEase(Ease.InCirc);
            }));
            sequence.Append(MagicCircle[2].rectTransform.DOScale(new Vector3(19f, 19f, 19f), 0.5f).SetEase(Ease.OutCubic));
            sequence.AppendInterval(0.4f);
            sequence.Append(MagicCircle[2].rectTransform.DOScale(new Vector3(18f, 18f, 18f), 0.5f).SetEase(Ease.InCirc).OnComplete(()=>MagicCircle[2].gameObject.SetActive(false)));
        }

        // FillAmount 감소
        private Tween _currentTween;
        private void DecreaseFill(float amount, float duration)
        {
            float target = Mathf.Clamp(MagicCircle[0].fillAmount - amount, 0, 1);
            if (_currentTween != null && _currentTween.IsActive())
            {
                _currentTween.Kill();
            }
            
            _currentTween = DOTween.To(()=> MagicCircle[0].fillAmount, x => MagicCircle[0].fillAmount = x, target, duration).SetEase(Ease.OutCubic);
        }
    }
}