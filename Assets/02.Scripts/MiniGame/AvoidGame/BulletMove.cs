using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace SeongIl
{

    public class BulletMove : MonoBehaviour
    {
        private Vector2 direction;
        public float Speed;
        

        // public bool Slow = true;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

            // Slow = true;
            direction = Vector2.left;
        }

        // Update is called once per frame
        void Update()
        {
            transform.Translate(direction * Speed * Time.deltaTime);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("DestroyZone"))
            {
                Destroy(gameObject);
            }
        }
    }
}
