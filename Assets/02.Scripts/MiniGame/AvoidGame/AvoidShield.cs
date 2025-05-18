using System.Collections;
using Com.LuisPedroFonseca.ProCamera2D;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

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
        private bool _isParrying = false;
        public Animator ParryingVFX;
        public GameObject[] ParryingVFXPrefab;
        public Image Flash;
        
        private SpriteRenderer _spriteRenderer;
        
        public Avoid Avoid;
        public bool PerfectShield = false;
        
        private void Start()
        {
            _playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            ShieldMovement();
            if (Input.GetKeyDown(KeyCode.Space) && PerfectShield)
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

            Vector3 movePos = _playerPosition.position + direction * (_shieldRadius + 0.5f);
            transform.position = Vector3.Lerp(transform.position, targetPos, _moveSpeed);
            
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);

            FlipShield();
            
            if (Input.GetKey(KeyCode.Space) && PerfectShield)
            {
                transform.position = Vector3.Lerp(transform.position, movePos, _moveSpeed);
            }

        
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Avoid"))
            {
                return;
            }
            
            ParryingVFXPrefab[1].SetActive(true);
            Destroy(other.gameObject);
            Avoid.SuccessCount += 1;
            
            if (Avoid.SuccessCount >= 10 && !PerfectShield)
            {
                PerfectShield = true;
                ParryingVFXPrefab[0].SetActive(true);
            }
            
            if (_isParrying && PerfectShield)
            {
                ParryingVFXPrefab[0].SetActive(true);
                StartCoroutine(FinishParrying());

            }
        }

        private IEnumerator Parrying()
        {   
            _isParrying = true;
            yield return new WaitForSeconds(_parryingTiming);
            _isParrying = false;
        }

        private IEnumerator FinishParrying()
        {
            Camera.main.DOOrthoSize(2.5f, 0.2f).OnComplete((() =>
            {
                Camera.main.DOOrthoSize(5f, 0.3f).SetEase(Ease.OutCirc);
            }));
            ParryingVFX.SetTrigger("Shock");
                
            while (Time.timeScale < 1)
            {
                
                Time.timeScale = 0.1f;
                yield return new WaitForSeconds(0.1f);
                Time.timeScale += 0.1f;
            }

            Flash.DOColor(new UnityEngine.Color(1f, 1f, 1f, 1f), 2f).SetEase(Ease.OutCirc);
            Avoid.GameStop();
            yield return new WaitForSeconds(1f);
            Avoid.ParryingSuccess();
        }


        private void FlipShield()   
        {
            if (transform.position.x < 0)
            {
                _spriteRenderer.flipY = true;
            }
            else
            {
                _spriteRenderer.flipY = false;
            }
        }

    }
    
    
}
