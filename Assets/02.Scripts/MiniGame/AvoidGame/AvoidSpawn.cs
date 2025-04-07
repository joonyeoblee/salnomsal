using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace SeongIl
{
    public class WarningData
    {
        public Vector2 Position;
        public float SpawnTime;

        public WarningData(Vector2 position, float spawnTime)
        {
            Position = position;
            SpawnTime = spawnTime;
            
        }
    }
    public class AvoidSpawn : MonoBehaviour
    {
        public Avoid ArrowCount;
        public float Timer = 0;
        public float SpawnTime = 2f;
        public float ShootingTime = 2f;
        //총알
        public GameObject Bullet;
        //경고 메세지
        public GameObject Warning;
        // 몇개 소환할거임?
        private int _bulletCount;
        // 위치 세팅하기용 vector
        public Vector2[] GetWayPoint;
        // 
        public Queue<WarningData> Warnings = new Queue<WarningData>();
        // private bool _timerStart = false;

        private Action ShotOn;

        private void Start()
        {
            _bulletCount = ArrowCount.ArrowCount;
            StartCoroutine(SpawnWarning());
        }

        // 위치와 타이밍 저장하고 경고 메세지 보여주기
        private IEnumerator SpawnWarning()
        {
            int randomCheck = 0;
            for (int i = 0; i < _bulletCount; i++)
            {
                yield return new WaitForSeconds(SpawnTime);
                int randPosition;
                do
                {
                    // 위치 저장하기
                    randPosition = UnityEngine.Random.Range(0, 3);
                    
                } while (randomCheck == randPosition);
                randomCheck = randPosition;
                // 시간과 위치 저장하기
                WarningData spawnData = new WarningData(GetWayPoint[randPosition], Timer);
                Warnings.Enqueue(spawnData);
                
                Debug.Log($"{spawnData.Position} {spawnData.SpawnTime}");
                Debug.Log(Warnings.Count);
                // 위치에 경고표시 보여주기
                GameObject waringPrefab = Instantiate(Warning, GetWayPoint[randPosition], Quaternion.identity);
                
                
                StartCoroutine(ShootingShot());
                
                yield return new WaitForSeconds(SpawnTime);
                
                
            }

        }

        private IEnumerator ShootingShot()
        {
                yield return new WaitForSeconds(ShootingTime);
                Debug.Log(Warnings.Count);
                Instantiate(Bullet, (Vector3)Warnings.Peek().Position, Quaternion.identity);
                Debug.Log("shotON");
                Warnings.Dequeue();
                ArrowCount.ArrowCount--;
        }
        
        // 버튼용
        public void Restart()
        {
            StartCoroutine(SpawnWarning());
        }
        
    }
}
