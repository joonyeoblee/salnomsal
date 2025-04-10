using System;
using System.Collections;
using UnityEngine;

namespace SeongIl
{
    public class AvoidShield : MonoBehaviour
    {
        private Transform _playerPosition;
        [SerializeField]
        private float _shieldRadius = 2f;
        [SerializeField]
        private float _moveSpeed = 1f;
        [SerializeField]
        private float _parryingTiming = 2f;
        [SerializeField]
        private int SuccessCount = 0;
        private bool _isParrying = false;   
        
        public Avoid Avoid;
        

        public int ParryingCount;
        
        private void Start()
        {
            _playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void Update()
        {
            ShieldMovement();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(Parrying());
            }
        }

        private void ShieldMovement()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            Vector3 direction = mousePos - _playerPosition.position;
            direction.Normalize();
            Vector3 targetPos = _playerPosition.position + direction * _shieldRadius;

            transform.position = Vector3.Lerp(transform.position, targetPos, _moveSpeed);
            
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
            
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Avoid"))
            {
                return;
            }
            
            Destroy(other.gameObject);
            Avoid.SuccessCount += 1;
            

            if (_isParrying)
            {
                Destroy(other.gameObject);
                ParryingCount += 1;
                Debug.Log($"패링 카운트 : {ParryingCount}");
                if (ParryingCount > SuccessCount)
                {
                    Avoid.ParryingSuccess();
                }
            }
        }

        private IEnumerator Parrying()
        {   
            _isParrying = true;
            yield return new WaitForSeconds(_parryingTiming);
            _isParrying = false;
        }
    }
}
