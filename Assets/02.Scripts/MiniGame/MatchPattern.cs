using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Jun.MiniGame
{
    public class MatchPattern : MonoBehaviour
    {
        // public static MatchPattern Instance;

        public GameObject Canvas;
        public GameObject[] Prefabs;

        public float MaxX;
        public float MaxY;

        [Header("갯수 설정")]
        public float MaxCount;
        [SerializeField] float _currentCount;

        [Header("게임 시작")]
        [SerializeField] bool _isGameActive;

        [Header("스폰 설정")]
        public float spawnInterval = 1.5f;

        private List<Magic> activeMagics = new();

        // void Awake()
        // {
        //     Instance = this;
        // }

        void Start()
        {
            _isGameActive = true;
            StartCoroutine(SpawnRoutine());
        }

        void Update()
        {
            if (_currentCount > MaxCount)
            {
                Success();
            }

            if (Input.anyKeyDown)
            {
                foreach (var magic in activeMagics)
                {
                    if (Input.GetKeyDown(magic.assignedKey.ToString().ToLower()))
                    {
                        magic.TryResolve();
                        break; // 하나만 처리
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
            if (Prefabs.Length == 0) return;

            GameObject prefab = Prefabs[Random.Range(0, Prefabs.Length)];

            Vector3 randomPosition = new Vector3(
                Random.Range(-MaxX, MaxX),
                Random.Range(-MaxY, MaxY),
                0f
            );

            GameObject spawned = Instantiate(prefab, transform);
            spawned.transform.localPosition = randomPosition;

            _currentCount++;
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
            _currentCount += 1;
        }

        public void Success()
        {
            Debug.Log("성공");
            _isGameActive = false;
        }
    }
}
