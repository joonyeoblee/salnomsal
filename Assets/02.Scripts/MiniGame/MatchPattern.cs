using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Jun.MiniGame
{
    public class MatchPattern : MonoBehaviour
    {
        public TextMeshProUGUI Text;
        private string _currentkey;
        public GameObject[] Prefabs;
        public Transform[] SpawnPoints;
        public Queue<GameObject> PrefabsQueue = new Queue<GameObject>();

        [Header("갯수 설정")]
        public float MaxCount;
        [SerializeField] float _currentCount;

        [Header("게임 시작")]
        [SerializeField] bool _isGameActive;

        [Header("스폰 설정")]
        public float spawnInterval = 1.5f; // 몇 초마다 스폰할지 설정

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
            if (Prefabs.Length == 0 || SpawnPoints.Length == 0) return;

            GameObject prefab = Prefabs[Random.Range(0, Prefabs.Length)];
            Transform spawnPoint = SpawnPoints[Random.Range(0, SpawnPoints.Length)];

            GameObject spawned = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
            PrefabsQueue.Enqueue(spawned);
            _currentCount++;
        }

        public void Fail()
        {
            Debug.Log("Fail");

            // _isGameActive = false;
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
