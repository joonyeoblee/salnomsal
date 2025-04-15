using Jun.Map;
using UnityEngine;

namespace Jun.Spawner
{
    public class MonsterSpawner : MonoBehaviour
    {
        public Transform[] spawnPositions; // 몬스터가 스폰될 위치 배열
        public GameObject[] monsterPrefabs; // 스폰할 몬스터 프리팹 배열

        public GameObject EliteMonster;
        public GameObject BossMonster;
        void OnEnable()
        {
            MapManager.Instance.OnMapNodeChanged += InitBattle;
        }

        void OnDisable()
        {
            MapManager.Instance.OnMapNodeChanged -= InitBattle;
        }
        public void InitBattle()
        {
            // 보스/엘리트 방이면 중앙에만 보스몹 스폰
            if (MapManager.Instance.currentNode.Type == NodeType.Boss)
            {
                // 보스 몬스터만 스폰
                SpawnRandomMonster(spawnPositions[1], BossMonster);

            } else if (MapManager.Instance.currentNode.Type == NodeType.Elite)
            {
                SpawnRandomMonster(spawnPositions[1], EliteMonster);
            } else
            {
                for (int i = 0; i < spawnPositions.Length; i++)
                {
                    GameObject monsterPrefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Length)];

                    // 일반 전투방인 경우 전체 포지션에 스폰
                    SpawnRandomMonster(spawnPositions[i], monsterPrefab);
                }
            }

        }
        public void SpawnRandomMonster(Transform spawnPoint, GameObject spawnPrefab)
        {
            if (spawnPositions == null || monsterPrefabs.Length == 0)
            {
                Debug.LogWarning("스폰 위치 또는 몬스터 프리팹이 설정되지 않았습니다.");
                return;
            }


            GameObject spawnedMonster = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation);
            Monster.Monster monster = spawnedMonster.GetComponent<Monster.Monster>();
            if (monster == null)
            {
                spawnedMonster.GetComponent<Monster.Boss>();
            }
            else
            {
                monster.MaxHealth += Random.Range(0, 10);
                monster.AttackPower += Random.Range(0, 10);
                monster.BasicSpeed += Random.Range(-1, 2);
                spawnedMonster.name = spawnPrefab.name + spawnPoint.name;
            }

            CombatManager.Instance.SpawnEnemy(spawnedMonster.GetComponent<EnemyCharacter>());
        }

    }
}
