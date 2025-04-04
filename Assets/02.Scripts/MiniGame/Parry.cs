using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SeongIl
{
    public class Parry : MonoBehaviour
    {
        // prototype 구현
        public GameObject Player;
        
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
        
        
        // 미니게임 시작 여부
        [SerializeField]
        private bool _isParrying;
        
        // 미니게임 이펙트
        public GameObject SlashEffect;
        
        // 패링 판단 여부 위치
        private Vector2 _successPosition;
        
        // 만들어진 오브젝트 타이밍 판정하기위한 리스트
        private List<GameObject> _timingChecks;
        private int _timingCheckIndex = 0;
        private void Start()
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            _successPosition = Player.transform.position;
            _timingChecks = new List<GameObject>(_count);
            _timingCheckIndex = 0;  

        }

        private void Update()
        {
            
            if (Input.GetKeyDown(KeyCode.Space) && _timingCheckIndex < _timingChecks.Count)
            {
                CheckTiming();
            }
            if (!_isParrying)
            {
                return;
            }
            
            StartCoroutine(ParryCount(_successPosition, 2));


        }
        
        
        //  슬래시 움직임주기
        private void SlashMovement(GameObject slash)
        {
            Debug.Log(Player.transform.position);
            Vector2 currentPosition = slash.transform.position;
            Vector2 oppositePosition = (currentPosition - _successPosition) * -1 + _successPosition;
            Debug.Log(oppositePosition);
            Debug.Log(currentPosition);
            slash.transform.DOMove(oppositePosition, _parrySpeed).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                slash.transform.DOMove(currentPosition, _parrySpeed).SetEase(Ease.OutCubic);
            });
   
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
                // 판정 개수 정하기
                _timingChecks.Add(slash);
                // 움직임 시작
                SlashMovement(slash);
                _isParrying = false;
                yield return new WaitForSeconds(_parryInstatiateTime);
            }

        }

        private void CheckTiming()
        {
            // x 축 y 축 판정 범위 설정
            float Xcheck = _timingChecks[_timingCheckIndex].transform.position.x - _successPosition.x;
            float Ycheck = _timingChecks[_timingCheckIndex].transform.position.y - _successPosition.y;

            if (Xcheck < _checkAmount && Xcheck > -_checkAmount && Ycheck < _checkAmount && Ycheck > -_checkAmount)
            {
                Success();
            }
            else
            {
                Fail();
            }
            
            _timingCheckIndex++;
                
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
            Debug.Log("Success");
            
        }
    }
}
