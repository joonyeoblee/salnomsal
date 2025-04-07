using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace SeongIl
{
    public class Parry : MonoBehaviour
    {
        // prototype 구현
        public GameObject Player;
        
        
        // flash효과
        public Image Flash;
        
        [Header("난이도 설정")]
        // 패링 타이밍 시간
        [SerializeField]
        private float _parrySpeed = 0;
        [SerializeField]
        private float _parryInstatiateTime = 0;
        [SerializeField]
        private int _count = 3;
        [SerializeField]
        private float _checkAmount = 0.3f;
        [SerializeField]
        private int _distance = 11;
        
        // 미니게임 시작 여부
        [SerializeField]
        private bool _gameStart = false;
        // 패링 중임? 
        public bool IsParried = false;
        
        // 미니게임 이펙트
        public GameObject SlashEffect;
        
        // 패링 판단 여부 위치
        private Vector2 _successPosition;
        
        // 판정 갯수세기 성공 여부 확인 위함
        private int _parriedCount = 0;
        private void Start()
        {
            _successPosition = transform.position;

        }

        private void Update()
        {
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(Parrying());
            }
            if (!_gameStart)
            {
                return;
            }
            
            StartCoroutine(ParryCount(_successPosition, _distance));


        }
        
        
        // //  슬래시 움직임주기
        // private void SlashMovement(GameObject slash)
        // {
        //     Debug.Log(Player.transform.position);
        //     Vector2 currentPosition = slash.transform.position;
        //     Vector2 oppositePosition = (currentPosition - _successPosition) * -1 + _successPosition;
        //     slash.transform.DOMove(oppositePosition, _parrySpeed).SetEase(Ease.OutCubic).OnComplete(() =>
        //     {
        //         slash.transform.DOMove(currentPosition, _parrySpeed).SetEase(Ease.OutCubic);
        //     });
        //
        // }

        // 슬래시 움직임 버전 2
        private void SlashMovement(GameObject slash)
        {
            Vector2 currentPosition = slash.transform.position; 
            Vector2 oppositePosition = (currentPosition - _successPosition) * -1 + _successPosition;
            Sequence sequence = DOTween.Sequence();
            sequence.Append(slash.transform.DOMove(oppositePosition, 0.1f).SetEase(Ease.OutCubic));
     
            sequence.AppendInterval(_parryInstatiateTime * _count + 1);
            
            sequence.Append(slash.transform.DOMove(currentPosition, _parrySpeed).SetEase(Ease.OutCubic).OnStart(() =>
            {
                StartCoroutine(FlashBackGround());
            }).OnComplete(() =>
            {
                slash.GetComponent<SlashChecker>()?.MissedCheck();
            }));        

            
        }
        
        // 슬래시 소환 위치 정하기 && 갯수 정하기 
        private IEnumerator ParryCount(Vector2 centerPosition, float distance)
        {
            for (int i = 0; i < _count; i++) // 예시 값
            {
                // 위치 정하기
                float angle = UnityEngine.Random.Range(0f, Mathf.PI *2);
                Vector2 pos =  new Vector2( centerPosition.x + distance * Mathf.Cos(angle), centerPosition.y + distance * Mathf.Sin(angle));
                GameObject slash = Instantiate(SlashEffect,pos, Quaternion.identity); 
                SlashChecker slashCheck = slash.AddComponent<SlashChecker>();
                slashCheck.OnMissed = Fail;
                // 움직임 시작
                SlashMovement(slash);
                _gameStart = false;
                yield return new WaitForSeconds(_parryInstatiateTime);
            }

        }

        // private void CheckTiming()
        // {
        //     // x 축 y 축 판정 범위 설정
        //     float Xcheck = _timingChecks[_timingCheckIndex].transform.position.x - _successPosition.x;
        //     float Ycheck = _timingChecks[_timingCheckIndex].transform.position.y - _successPosition.y;
        //
        //     if (Xcheck < _checkAmount && Xcheck > -_checkAmount && Ycheck < _checkAmount && Ycheck > -_checkAmount)
        //     {
        //         Success();
        //     }
        //     
        //     _timingCheckIndex++;
        //         
        // }
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Avoid") && IsParried)
            {
              Success();   
              Destroy(other.gameObject);
            }
        }

        // 판정
        private void Fail()
        {
            Debug.Log("Fail");
            
            //for (int i = _timingChecks.Count - 1; i >= 0; i--)
            //{
            //    Destroy(_timingChecks[i]);
            //    _timingChecks.RemoveAt(i);
            //}

        }
        
        // 성공
        private void Success()
        {
            _parriedCount += 1;
            Debug.Log("Parried");
            
            if (_parriedCount >= _count)
            {
                Debug.Log("Success");
            }
        }

        private IEnumerator FlashBackGround()
        {
            yield return new WaitForSeconds(0.25f);
            Flash.DOColor(new Color(1, 1, 1, 0.2f), 0.2f).OnComplete(() =>
            {
                Flash.DOColor(new Color(1, 1, 1, 0f), 0.2f);
            });
        }

        private IEnumerator Parrying()
        {
            IsParried = true;
            yield return new WaitForSeconds(0.1f);
            IsParried = false;
        }
    }
}
