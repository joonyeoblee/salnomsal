using System;
using System.Collections;
using NUnit.Framework.Constraints;
using UnityEngine;

namespace SeongIl
{


    public class Avoid : MonoBehaviour
    {
        // Player포지션 받아오기
        public GameObject Player;

        // player 이동 범위 정하기
        public int[] PositionY = new int[2];

        // 움직임 구현
        private int _speed = 3;

        // 난이도 구현

        // 미니게임 시작 여부 (움직임 가능 여부)
        private bool _isMoving = false;

        private void Start()
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            

        }

        // 피하기 시작하기
        private void Update()
        {
            if (!_isMoving)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                Player.transform.position += Vector3.up;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                Player.transform.position += Vector3.down;
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
        private void SetRangeLimit()
        {

        }

        // 로컬 포지션 정하기
        private void SetCoords(int x, int y)
        {

        }

        // private IEnumerator Move(Vector2 dir)
        // {
        //     
        // }


    }
}
