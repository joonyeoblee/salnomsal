
using System;
using UnityEngine;

namespace SeongIl
{

    public class PlayerCollision : MonoBehaviour
    {
        // 판정 전달
        public Avoid Avoid;
        private int _hitCount = 0;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Avoid"))
            {
                return;
            }
            Destroy(other.gameObject);
            Debug.Log(_hitCount);
            
            if (_hitCount > Avoid.Lives)
            {
                GameObject[] warning = GameObject.FindGameObjectsWithTag("Warning");
                GameObject[] bullets = GameObject.FindGameObjectsWithTag("Avoid");
                for (int i = 0; i < warning.Length; i++)
                {
                    Destroy(bullets[i]);
                    Destroy(warning[i]);
                }
                Debug.Log("콜리전 충돌 넘 많아 실패");

                Avoid.Fail();
            }
            else
            {
                _hitCount += 1;
                Avoid.SuccessCount += 1;
            }
        }
    }
        
}