using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jun.MiniGame
{
    public class MatchPattern : MonoBehaviour
    {
        private string _currentkey;
        public GameObject[] Prefabs;
        public Transform[] SpawnPoints;
        public Queue<GameObject> PrefabsQueue = new Queue<GameObject>();

        readonly HashSet<Transform> occupiedSpawnPoints = new HashSet<Transform>();

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

            // 사용 가능한 스폰포인트만 필터링
            List<Transform> availablePoints = new List<Transform>();
            foreach (Transform point in SpawnPoints)
            {
                if (!occupiedSpawnPoints.Contains(point))
                {
                    availablePoints.Add(point);
                }
            }

            if (availablePoints.Count == 0)
            {
                Debug.Log("사용 가능한 스폰포인트가 없습니다.");
                return;
            }

            GameObject prefab = Prefabs[Random.Range(0, Prefabs.Length)];
            Transform spawnPoint = availablePoints[Random.Range(0, availablePoints.Count)];

            GameObject spawned = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
            PrefabsQueue.Enqueue(spawned);
            _currentCount++;

            occupiedSpawnPoints.Add(spawnPoint);

            // Magic 스크립트에서 해제 시 호출하게 설정
            Magic magic = spawned.GetComponent<Magic>();
            if (magic != null)
            {
                magic.SetSpawnPoint(spawnPoint);
                magic.OnDestroyed += ReleaseSpawnPoint;
            }
        }

        void ReleaseSpawnPoint(Transform spawnPoint)
        {
            if (occupiedSpawnPoints.Contains(spawnPoint))
            {
                occupiedSpawnPoints.Remove(spawnPoint);
            }
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
