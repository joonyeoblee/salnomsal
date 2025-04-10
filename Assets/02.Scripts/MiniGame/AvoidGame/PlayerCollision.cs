
using System;
using Unity.VisualScripting;
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
            
            if (_hitCount > 3)
            {
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