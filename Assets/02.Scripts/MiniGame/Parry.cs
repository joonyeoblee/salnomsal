using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;
using Random = System.Random;

namespace SeongIl
{
    public class Parry : MonoBehaviour
    {
        // prototype 구현
        // 패링 타이밍 시간
        private float _parryTime;
        private float _parryTimer = 0;
        // 미니게임 시작 여부
        [SerializeField]
        private bool _isParrying;
        
        // 미니게임 인터페이스
        public GameObject SlashEffect;
        
        // 패링 판단 여부 위치
        private Vector2 _successPosition;

        private void Start()
        {
            _successPosition = transform.position;
        }

        private void Update()
        {
            if (!_isParrying)
            {
                return;
            }

            SlashStartPosition(transform.position, 2f);
        }
        
        // 슬래시 소환 위치 정하기
        private void SlashStartPosition(Vector2 centerPosition, float distance)
        {
            float angle = UnityEngine.Random.Range(0f, 2f);
            float x = centerPosition.x + distance * Mathf.Cos(angle);
            float y = centerPosition.y + distance * Mathf.Sin(angle);
            centerPosition =  new Vector2(x, y);
            Instantiate(SlashEffect, centerPosition, Quaternion.identity);  
        }
        
        

        // 판정
        private void Fail()
        {
            Debug.Log("Fail");
        }
        
        // 성공
        private void Success()
        {
            Debug.Log("Success");   
        }
    }
}
