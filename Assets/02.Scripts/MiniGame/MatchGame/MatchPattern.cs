using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Jun;
using Mono.Cecil.Cil;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace SeongIl
{
    public class MatchPattern : MonoBehaviour
    {
        public Image Book; 
        public TextMeshProUGUI Text;
        private string _currentkey;
        // 이펙트
        public Animator MagicEffect;
        
        public Image MagicCircle;
        
        
        
        [Header("시간 설정")]
        public float TimeLimit = 0;
        public float BonusDecline;
        public float IncreaseTime;
        public float DeclineTime;
        
        [Header("게임 시작")]
        public bool _isGameActive = true;
        
        private Queue<string> _keyQueue = new Queue<string>();

        public int KeyCount = 5;
        

        private void Update()
        {
            
            if (!_isGameActive)
            {
                return;
            }
            
            MagicCircle.fillAmount += TimeLimit * Time.deltaTime;
            
            if (MagicCircle.fillAmount >= 1)
            {
                Fail();
            }
            else if (MagicCircle.fillAmount <= 0.007f)
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

                        MagicCircle.fillAmount -= DeclineTime;
                        DecreaseFill(DeclineTime, 0.1f);
                        StartCoroutine(ShakeObjetct(MagicCircle, 3, -0.05f,0.05f,0.1f));
                        StartCoroutine(ShakeObjetct(Book, 3, 0.05f,0.05f, 0.1f));
                    }
                    else
                    {
                        Debug.Log("Reload");
                        GenerateKeysQueue();
                        DisplayKeys();
                        MagicCircle.fillAmount -= BonusDecline;
                    }
                    MagicEffect.SetTrigger("Key");
                }
                else
                {
                    MagicCircle.fillAmount += IncreaseTime;
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
            
            MiniGameScenesManager.instance.Fail?.Invoke();
            Scene sceneToUnload = SceneManager.GetSceneAt(1); // 로드된 씬 중 두 번째 (0은 기본 active 씬)
            SceneManager.UnloadSceneAsync(sceneToUnload);
            
        }

        public void Success()
        {
            Debug.Log("성공");
            _isGameActive = false;

            MiniGameScenesManager.instance.Success?.Invoke();
            Scene sceneToUnload = SceneManager.GetSceneAt(1); // 로드된 씬 중 두 번째 (0은 기본 active 씬)
            SceneManager.UnloadSceneAsync(sceneToUnload);
        }

        public IEnumerator GameStart()
        {
            yield return new WaitForSeconds(4f);
            GenerateKeysQueue();
            DisplayKeys();
            MagicCircle.fillAmount = 0.1f;
            _isGameActive = true;
        }

        private IEnumerator ShakeObjetct(Image circle, int count, float min, float max, float duration)
        {
            Vector2 origin = circle.rectTransform.anchoredPosition;
            Debug.Log(origin);
            Vector2 f = circle.rectTransform.position;
            Debug.Log(f);
            Vector2 p = circle.rectTransform.localPosition;
            Debug.Log(p);
            for (int i = 0; i < count; i++)
            {
                float randX = Random.Range(min , max);
                float randY = Random.Range(min, max);
                circle.rectTransform.DOMove(new Vector3(origin.x + randX ,origin.y +randY, 0), duration);
                yield return new WaitForSeconds(duration);
                
            }
            
            circle.rectTransform.anchoredPosition = origin;
        }
        
        private Tween _currentTween;
        private void DecreaseFill(float amount, float duration)
        {
            float target = Mathf.Clamp(MagicCircle.fillAmount - amount, 0, 1);
            if (_currentTween != null && _currentTween.IsActive())
            {
                _currentTween.Kill();
            }
            
            _currentTween = DOTween.To(()=> MagicCircle.fillAmount, x => MagicCircle.fillAmount = x, target, duration).SetEase(Ease.OutCubic);
        }
    }
}