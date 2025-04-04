using System.Collections;
using UnityEngine;

namespace Jun.MiniGame
{
    public class Magic : MonoBehaviour
    {
        public char assignedKey; // q, w, e, r 중 하나를 할당받음
        bool _isKeyPressed;
        MatchPattern _matchPattern;

        void Start()
        {
            // 1초 내에 키가 눌리지 않으면 Fail 호출
            StartCoroutine(CheckKeyTimeout());

            // MatchPattern 찾아서 참조
            _matchPattern = FindObjectOfType<MatchPattern>();
        }

        void Update()
        {
            if (!_isKeyPressed && Input.anyKeyDown)
            {
                if (Input.GetKeyDown(assignedKey.ToString().ToLower()))
                {
                    _isKeyPressed = true;
                    Debug.Log($"정확한 키 {assignedKey} 입력!");
                    Destroy(gameObject);

                    // 성공 로직이 필요하면 여기에 추가
                }
            }
        }

        IEnumerator CheckKeyTimeout()
        {
            yield return new WaitForSeconds(1f);

            if (!_isKeyPressed)
            {
                Debug.Log($"키 입력 실패: {assignedKey}");
                if (_matchPattern != null)
                    _matchPattern.Fail();
            }
        }
    }
}
