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

            float radian = Mathf.Atan2(_direction.y, _direction.x);
            float angle = Mathf.Rad2Deg * radian - 135f ;

            // 회전 적용 (Z축 기준, 2D 전용)
            transform.eulerAngles = new Vector3(0, 0, angle);
        }
        void Update()
        {
            transform.position += (Vector3)_direction * Speed * Time.deltaTime;
        }
        
    }
}
