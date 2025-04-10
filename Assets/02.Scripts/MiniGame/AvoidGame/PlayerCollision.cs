
using System;
using Unity.VisualScripting;
using UnityEngine;

namespace SeongIl
{

    public class PlayerCollision : MonoBehaviour
    {
        // 판정 전달
        public Avoid Avoid;
        // 스포너 끄고싶어
        public GameObject Spawner;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Avoid"))
            {
                if (Avoid.Parrying)
                {
                    Avoid.ParryingSuccess();
                    Spawner.SetActive(false);
                    this.enabled = false;
                }
                else
                {
                    
                    Avoid.Fail();
                    Spawner.SetActive(false);
                    this.enabled = false;
                }
            }
        }
        
    }
}