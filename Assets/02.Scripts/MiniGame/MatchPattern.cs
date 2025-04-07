using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

namespace SeongIl
{
    public class MatchPattern : MonoBehaviour
    {
        public TextMeshProUGUI Text;
        private string _currentkey;
        
        public Image MagicCircle;
        
        [Header("시간 설정")]
        public float TimeLimit = 0;
        public float Timer;
        public float DeclineTime;
        [Header("게임 시작")] private bool _isGameActive = true;
        
        private Queue<string> _keyQueue = new Queue<string>();

        public int KeyCount = 5;
        

        private void Start()
        {
            GenerateKeysQueue();
            DisplayKeys();
            MagicCircle.fillAmount = 0;
        }

        private void Update()
        {
            
            if (!_isGameActive)
            {
                return;
            }
            MagicCircle.fillAmount += 0.5f * Time.deltaTime;
            if (MagicCircle.fillAmount > 1)
            {
                _isGameActive = false;
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
                        Success();                    }
                }
                else
                {
                    Fail();
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
            //isGameActive = false;
            
        }

        public void Success()
        {
            Debug.Log("성공");
        }
    }
}