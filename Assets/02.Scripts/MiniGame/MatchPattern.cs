using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Jun.MiniGame
{
    public class MatchPattern : MonoBehaviour
    {

        public GameObject Canvas;
        public GameObject[] Prefabs;

        public float MaxX;
        public float MaxY;

        [Header("갯수 설정")]
        public float MaxCount;
        [SerializeField] int _currentCount;
        [SerializeField] int _spawnedCount; // 따로 관리

        [Header("게임 시작")]
        [SerializeField] bool _isGameActive;

        [Header("스폰 설정")]
        public float spawnInterval = 1.5f;

        private List<Magic> activeMagics = new();

        void Start()
        {
            _isGameActive = true;
            StartCoroutine(SpawnRoutine());
        }

        void Update()
        {
            // if (_currentCount > MaxCount)
            // {
            //     Success();
            // }
            
            foreach (KeyCode kc in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(kc))
                {
                    string keyStr = kc.ToString().ToLower();
                    if (keyStr.Length == 1)
                    {
                        char inputChar = keyStr[0];

                        foreach (var magic in activeMagics)
                        {
                            if (magic.assignedKey == inputChar)
                            {
                                magic.TryResolve();
                                break; // 가장 먼저 매칭된 하나만!
                            }
                        }

                        break; // 여러 키 누르면 그 중 하나만 처리
                    }
                }
            }
        }

        IEnumerator SpawnRoutine()
        {
            while (_isGameActive)
            {
                SpawnRandomPrefab();
                yield return new WaitForSeconds(spawnInterval);
            }
        }

        void SpawnRandomPrefab()
        {
            if (!_isGameActive || _spawnedCount >= MaxCount) return;
            if (Prefabs.Length == 0) return;

            GameObject prefab = Prefabs[Random.Range(0, Prefabs.Length)];
            RectTransform prefabRect = prefab.GetComponent<RectTransform>();

            Vector2 randomPos;
            int attempt = 0;
            const int maxAttempts = 30;

            do
            {
                randomPos = new Vector2(
                    Random.Range(-MaxX, MaxX),
                    Random.Range(-MaxY, MaxY)
                );

                attempt++;

                if (IsPositionClear(randomPos, prefabRect))
                    break;

            } while (attempt < maxAttempts);

            if (attempt >= maxAttempts)
            {
                Debug.LogWarning("겹치지 않는 위치 찾기 실패");
            }

            GameObject spawned = Instantiate(prefab, transform);
            RectTransform rt = spawned.GetComponent<RectTransform>();
            rt.anchoredPosition = randomPos;

            _spawnedCount++;
        }
        bool IsPositionClear(Vector2 candidatePos, RectTransform prefabRect)
        {
            foreach (var magic in activeMagics)
            {
                if (magic == null) continue;

                RectTransform existingRect = magic.GetComponent<RectTransform>();

                if (IsOverlapping(existingRect, existingRect.anchoredPosition, prefabRect, candidatePos))
                    return false;
            }

            return true;
        }



        bool IsOverlapping(RectTransform a, Vector2 aPos, RectTransform b, Vector2 bPos)
        {
            Vector2 aSize = a.sizeDelta;
            Vector2 bSize = b.sizeDelta;

            float aLeft = aPos.x - aSize.x / 2;
            float aRight = aPos.x + aSize.x / 2;
            float aTop = aPos.y + aSize.y / 2;
            float aBottom = aPos.y - aSize.y / 2;

            float bLeft = bPos.x - bSize.x / 2;
            float bRight = bPos.x + bSize.x / 2;
            float bTop = bPos.y + bSize.y / 2;
            float bBottom = bPos.y - bSize.y / 2;

            return !(aRight < bLeft || aLeft > bRight || aTop < bBottom || aBottom > bTop);
        }



        public void RegisterMagic(Magic magic)
        {
            if (!activeMagics.Contains(magic))
                activeMagics.Add(magic);
        }

        public void UnregisterMagic(Magic magic)
        {
            if (activeMagics.Contains(magic))
                activeMagics.Remove(magic);
        }

        public void Fail()
        {
            Debug.Log("Fail");
        }

        public void AddCount()
        {
            _currentCount++;

            if (_currentCount >= MaxCount)
            {
                Success();
            }
        }


        public void Success()
        {
            Debug.Log("성공");
            _isGameActive = false;
            
        }

    }
}
