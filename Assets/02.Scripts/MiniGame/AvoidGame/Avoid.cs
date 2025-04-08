using System.Collections;
using Jun;
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

        // 움직임 구현 (이속, 범위)
        private float _endTime = 0.05f;
        private float _maxLimit;
        private float _minLimit;

        // 패링 시간 
        [SerializeField]
        private float _parryTime = 0.5f;
        // 움직임 가능 여부
        [SerializeField]
        private bool _isMoving = true;

        // 패링시스템 (패링 치기)
        public bool Parrying = false;
        
        // 화살 다 피함? 카운트 세기
        public int ArrowCount = 10;
        // 게임 시작
        public bool _isGameOver = false;
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
            if (_isGameOver)
            {
                return;
            }
            // 게임 끝내기
            if (ArrowCount <= 0)
            {
                Success();
            }
            // 조작
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
            yield return new WaitForSeconds(_parryTime);
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

        // 실패   
        public void Fail()
        {
            Debug.Log("Fail");
            MiniGameScenesManager.instance.Fail?.Invoke();
        }
        // 성공
        public void Success()
        {
            Debug.Log("Success");
            _isGameOver = true;

            MiniGameScenesManager.instance.Success?.Invoke();
        }
        // 패링 성공
        public void ParryingSuccess()
        {
            ParrySound.PlayOneShot(ParrySound.clip);
            Debug.Log("ParryingSuccess");
            _isGameOver = true;
            MiniGameScenesManager.instance.Parring?.Invoke();
        }
    }
}
