using System.Collections.Generic;
using Jun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SeongIl
{
    public class MatchPattern : MonoBehaviour
    {
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
                int index = UnityEngine.Random.Range(0, keys.Length);
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
            
        }

        public void Success()
        {
            Debug.Log("성공");
            _isGameActive = false;
            
            MiniGameScenesManager.instance.Sucess?.Invoke();
        }

        public void GameStart()
        {
            GenerateKeysQueue();
            DisplayKeys();
            MagicCircle.fillAmount = 0.1f;
            _isGameActive = true;
        }
    }
}