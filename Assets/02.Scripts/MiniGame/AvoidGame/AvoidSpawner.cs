using System.Collections;
using UnityEngine;

namespace SeongIl 
{
public class AvoidSpawner : MonoBehaviour
{
    private int SpawnCount;
    public float Distance = 4f;
    private float MinTime;
    private float MaxTime;
    public Vector3 SpawnPos;
    public GameObject Bullet;
    public GameObject Warning;

    private void Start()
    {   
        SpawnPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        
    }
    
    //난이도 세팅
    public void DifficultySet(Avoid avoid)
    {
        MaxTime = avoid.MaxTime;
        MinTime = avoid.MinTime;
    }
    // 위치 정하기
    private IEnumerator SpawnPositionSet()
    {   
        for (int i = 0; i < SpawnCount; i++)
        {
            float randomSpawn = UnityEngine.Random.Range(MinTime, MaxTime);
            
            int randomPosition = UnityEngine.Random.Range(0, 18);

            float angle = randomPosition * 20f;
            float degAngle = angle *  Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Cos(degAngle), Mathf.Sin(degAngle), 0f);
            Vector3 spawn = SpawnPos + direction * Distance;
            
            Instantiate( Warning, spawn , Quaternion.Euler(0f, 0f, angle));
            
            yield return new WaitForSeconds(randomSpawn);
            
            Instantiate(Bullet, spawn, Quaternion.identity);

        }
        
    }

    //게임 시작과 동시에 카운트를 받아
    public void SpawnStart( int count, Avoid avoid)
    {
        DifficultySet(avoid);
        SpawnCount = count;
        StartCoroutine(SpawnPositionSet());
    }

}
}
