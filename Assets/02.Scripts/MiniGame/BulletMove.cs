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

        // public GameObject Player;

        // public bool Slow = true;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            // Player = GameObject.FindGameObjectWithTag("Player");
            // Slow = true;
            direction = Vector2.left;
        }

        // Update is called once per frame
        void Update()
        {
            transform.Translate(direction * Speed * Time.deltaTime);

            /*if (Mathf.Abs(transform.position.y - Player.transform.position.y) > 1.95f )
            {
                return;
            }

            if (transform.position.x < Player.transform.position.x + 1f )
            {
                Time.timeScale = 0.01f;
                StartCoroutine(TimeScaleOff());
            }*/
        }

        private IEnumerator TimeScaleOff()
        {
            yield return new WaitForSecondsRealtime(0.3f);
            Time.timeScale = 1f;
            // Slow = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("A"))
            {
                Destroy(gameObject);
            }
        }
    }
}
