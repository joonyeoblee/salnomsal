using System;
using System.Collections;
using DG.Tweening;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;
using Color = System.Drawing.Color;

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
        private int SuccessCount = 2;
        private bool _isParrying = false;
        public Animator ParryingVFX;
        public GameObject ParryingVFXPrefab;
        public Image Flash;
        
        
        public Avoid Avoid;
        

        public int ParryingCount;
        
        private void Start()
        {
            _playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void Update()
        {
            ShieldMovement();
            if (Input.GetKeyDown(KeyCode.Space) && !_isParrying)
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
            Debug.Log($"shield 히트 +1 {Avoid.SuccessCount}");

            if (_isParrying)
            {
                Destroy(other.gameObject);
                ParryingCount += 1;
                Debug.Log($"패링 카운트 : {ParryingCount}");
                ParryingVFXPrefab.SetActive(true);
                if (ParryingCount > SuccessCount)
                {
                    StartCoroutine(FinishParrying());
                }
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
            Camera.main.DOOrthoSize(3f, 0.2f).OnComplete((() =>
            {
                Camera.main.DOOrthoSize(5f, 0.5f).SetEase(Ease.OutCirc);
            }));
            ParryingVFX.SetTrigger("Shock");
                
            while (Time.timeScale >= 1)
            {
                
                Time.timeScale = 0.1f;
                yield return null;
                Time.timeScale += 0.3f;
            }

            Flash.DOColor(new UnityEngine.Color(1f, 1f, 1f, 0.7f), 0.7f).SetEase(Ease.OutCirc).OnComplete(() =>
            {
                DOTween.KillAll();
                Debug.Log("다 죽임: 쉴드");
            });
            
            yield return null;
            Avoid.ParryingSuccess();
        }
        //
        // private void FlashBang()
        // {
        //     Flash.DOColor(new UnityEngine.Color(1f, 1f, 1f, 0.7f), 0.2f).SetEase(Ease.OutCirc).OnComplete(() =>
        //     {
        //         
        //     });
        //     
        // }
    }
    
    
}
