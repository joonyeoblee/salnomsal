using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace SeongIl
{

    public class BulletMove : MonoBehaviour
    {
        private Vector2 _direction;
        public float Speed;
        private GameObject MyPlayer;
        void Start()
        {
            MyPlayer = GameObject.FindGameObjectWithTag("Player");
            _direction = MyPlayer.transform.position - transform.position;
            _direction.Normalize();
            
        }
        void Update()
        {
            transform.Translate(_direction * Speed * Time.deltaTime);
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
