using System.Collections;
using System.Threading;
using UnityEngine;

namespace SeongIl
{


    public class Avoid : MonoBehaviour
    {
        // Player포지션 받아오기
        public GameObject Player;

        // player 이동 범위 정하기
        private Vector2 _currentPosition;
        private int _movePositionAmount = 2;
        private float _maxLimit = 0;
        private float _minLimit = 0;
        
        // 움직임 구현
        private float _endTime = 0.05f;
        // 난이도 구현
        
        // 미니게임 시작 여부 (움직임 가능 여부)
        [SerializeField]
        private bool _isMoving = true;

        private void Start()
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            _currentPosition = Player.transform.position;
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
                StartCoroutine(Move(2f));
            }
            else if (Input.GetKeyDown(KeyCode.S) && Player.transform.position.y > _minLimit)
            {
                StartCoroutine(Move(-2f));
            }

        }

        // 충돌체크하기
        private void OnColliderEnter2D(Collider2D other)
        {
            if (other.CompareTag("Avoid"))
            {
                Debug.Log("Fail");
            }
        }

        // 움직임 제한하기
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

            
    }
}
