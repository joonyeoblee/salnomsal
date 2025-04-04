using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

namespace SeongIl
{
    public class MatchPattern : MonoBehaviour
    {
        public TextMeshProUGUI Text;
        private string _currentkey;
        public GameObject[] Prefabs;
        public Queue<GameObject> PrefabsQueue = new Queue<GameObject>();
        [Header("시간 설정")]
        public float TimeLimit = 0;
        public float Timer;
        [Header("게임 시작")] private bool _isGameActive = true;
        
        private Queue<string> _keyQueue = new Queue<string>();

        public int KeyCount = 5;
        

        private void Start()
        {
            GenerateKeysQueue();
            DisplayKeys();
        }

        private void Update()
        {
            if (!_isGameActive)
            {
                return;
            }
            Timer -= Time.deltaTime;
            if (Timer <= 0)
            {
                _isGameActive = false;
            }

            if (Input.anyKeyDown)
            {
                if (Input.GetKeyDown(_keyQueue.Peek().ToLower()))
                {
                    _keyQueue.Dequeue();
                    PrefabsQueue.Dequeue().GetComponent<SpriteRenderer>().color = Color.black;
                    if (_keyQueue.Count > 0)
                    {
                        DisplayKeys();
                        
                    }
                    else
                    {
                        Success();            }
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
                GameObject obj = Instantiate(Prefabs[index],new Vector3(transform.position.x + i, transform.position.y, 0), Quaternion.identity);
                PrefabsQueue.Enqueue(obj);
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