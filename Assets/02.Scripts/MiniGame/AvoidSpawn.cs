using System;
using System.Collections;
using System.Collections.Generic;
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
        public float Timer = 0;
        public float SpawnTime = 2f;
        public GameObject Bullet;
        public GameObject Warning;
        private int _bulletCount = 0;
        public List<Vector2> Waypoints = new List<Vector2>();
        public List<WarningData> Warnings = new List<WarningData>();

        private void Start()
        {
            
        }

        private void Update()
        {
            Timer += Time.deltaTime;
            if (Timer < SpawnTime)
            {
                return;
            }
            Instantiate(Bullet, transform.position, transform.rotation);
            Timer = 0;
        }

        private IEnumerator SpawnWarning()
        {
            for (int i = 0; i < _bulletCount; i++)
            {
                float randPosition = UnityEngine.Random.Range(0, 2);
                
            }
            yield return new WaitForSeconds(SpawnTime);
        }
    }
}
