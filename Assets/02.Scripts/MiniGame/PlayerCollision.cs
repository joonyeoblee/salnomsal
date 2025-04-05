
using System;
using Unity.VisualScripting;
using UnityEngine;

namespace SeongIl
{

    public class PlayerCollision : MonoBehaviour
    {
        public Avoid Avoid;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Avoid"))
            {
                if (Avoid.Parrying)
                {
                    Avoid.Success();
                }
                else
                {
                    
                    Avoid.Fail();
                }
            }
        }
    }
}