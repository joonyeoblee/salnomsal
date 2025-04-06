using System.Collections;
using System.Threading;
using UnityEngine;

namespace SeongIl
{


    public class Avoid : MonoBehaviour
    {
        // Player포지션 받아오기
        public GameObject Player;
        //  parrying 사운드
        public AudioSource ParrySound;
        // player 이동 범위 정하기
        private Vector2 _currentPosition;
        private int _movePositionAmount = 2;
        private float _maxLimit = 0;
        private float _minLimit = 0;
        
        // 움직임 구현
        private float _endTime = 0.05f;

        // 움직임 가능 여부
        [SerializeField]
        private bool _isMoving = true;

        // 패링시스템
        public bool Parrying = false;
        
        private void Start()
        {
            //플레이어 찾기
            Player = GameObject.FindGameObjectWithTag("Player");
            //플레이어 시작지점 찾기
            _currentPosition = Player.transform.position;
            //범위 설정
            _maxLimit = _currentPosition.y + _movePositionAmount;
            _minLimit = _currentPosition.y - _movePositionAmount;

        }

        // 피하기 시작하기
        private void Update()
        {
            if (!_isMoving)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.W) && Player.transform.position.y < _maxLimit )
            {
                StartCoroutine(Move(_movePositionAmount));
            }
            else if (Input.GetKeyDown(KeyCode.S) && Player.transform.position.y > _minLimit)
            {
                StartCoroutine(Move(-_movePositionAmount));
            }
            else if (Input.GetKeyDown(KeyCode.Space) && !Parrying)
            {
                StartCoroutine(ParryingTiming());
            }

        }

        // 충돌체크하기

        
        //판정 구하기
        private IEnumerator ParryingTiming()
        {
            Parrying = true;
            yield return new WaitForSeconds(0.05f);
            Parrying = false;
        }

        // 움직임 구현하기
        private IEnumerator Move(float y)
        {
            _isMoving = false;
            
            float startTime = 0;
            
            Vector2 current = Player.transform.position;
            Vector2 target = current + new Vector2(0, y);

            while (startTime < _endTime)
            {
                Player.transform.position = Vector2.Lerp(current, target, startTime / _endTime);
                startTime += Time.deltaTime;
                yield return null;
            }

            Player.transform.position = target;
            _isMoving = true;
        }

        public void Fail()
        {
            Debug.Log("Fail");
        }

        public void Success()
        {
            ParrySound.PlayOneShot(ParrySound.clip);
            
            Debug.Log("Success");
        }
    }
}
