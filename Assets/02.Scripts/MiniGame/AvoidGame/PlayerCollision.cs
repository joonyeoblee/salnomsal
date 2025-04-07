
using System;
using Unity.VisualScripting;
using UnityEngine;

namespace SeongIl
{

    public class PlayerCollision : MonoBehaviour
    {
        public Avoid Avoid;
        public GameObject Spawner;
        
        private void OnTriggerStay2D(Collider2D other)
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