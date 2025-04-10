using System;
using System.Collections;
using UnityEngine;
using Random = System.Random;

namespace SeongIl
{
public class AvoidSpawner : MonoBehaviour
{
    public int SpawnCount;
    public float SpawnTime = 2f;
    public float Distance = 4f;
    public float MinTime;
    public float MaxTime;
    public Vector3 SpawnPos;
    public GameObject Bullet;

    private void Start()
    {   
        SpawnPos = GameObject.FindGameObjectWithTag("Player").transform.position;

        StartCoroutine(SpawnPositionSet());
    }
    // 위치 정하기
    private IEnumerator SpawnPositionSet()
    {
        for (int i = 0; i < SpawnCount; i++)
        {
            float randomSpawn = UnityEngine.Random.Range(MinTime, MaxTime);
            
            int randomPosition = UnityEngine.Random.Range(0, 18);

            float angle = randomPosition * 20f  * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);
            Vector3 spawn = SpawnPos + direction * Distance;
            
            Instantiate(Bullet, spawn, Quaternion.identity);
            
            
            yield return new WaitForSeconds(randomSpawn);
        }
        
    }
}
}
