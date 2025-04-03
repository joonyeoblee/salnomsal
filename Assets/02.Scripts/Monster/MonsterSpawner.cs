using System.Collections.Generic;
using UnityEngine;

namespace Jun.Spawner
{
    public class MonsterSpawner : MonoBehaviour
    {
        public Transform[] spawnPositions; // 몬스터가 스폰될 위치 배열
        public GameObject[] monsterPrefabs; // 스폰할 몬스터 프리팹 배열
        readonly HashSet<Transform> occupiedPositions = new HashSet<Transform>(); // 이미 사용된 위치 추적

        void Start()
        {
            SpawnRandomMonster();
            SpawnRandomMonster();
            SpawnRandomMonster();
        }
        public void SpawnRandomMonster()
        {
            if (spawnPositions.Length == 0 || monsterPrefabs.Length == 0)
            {
                Debug.LogWarning("스폰 위치 또는 몬스터 프리팹이 설정되지 않았습니다.");
                return;
            }

            // 사용 가능한 위치 필터링
            List<Transform> availablePositions = new List<Transform>();
            foreach (Transform pos in spawnPositions)
            {
                if (!occupiedPositions.Contains(pos))
                    availablePositions.Add(pos);
            }

            // 모든 위치가 사용 중이라면 스폰하지 않음
            if (availablePositions.Count == 0)
            {
                Debug.LogWarning("모든 스폰 위치가 이미 사용 중입니다.");
                return;
            }

            // 랜덤한 위치 선택
            Transform spawnPoint = availablePositions[Random.Range(0, availablePositions.Count)];

            // 랜덤한 몬스터 선택
            GameObject monsterPrefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Length)];

            // 몬스터 생성
            GameObject spawnedMonster = Instantiate(monsterPrefab, spawnPoint.position, spawnPoint.rotation);

            // 생성된 위치를 사용 중으로 표시
            occupiedPositions.Add(spawnPoint);
        }
    }
}
