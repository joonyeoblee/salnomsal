using System;
using System.Collections;
using UnityEngine;

namespace Jun.MiniGame
{
    public class Magic : MonoBehaviour
    {
        public char assignedKey;
        private bool _isKeyPressed = false;
        private MatchPattern _matchPattern;
        private Transform _mySpawnPoint;

        public event Action<Transform> OnDestroyed;

        void Start()
        {
            _matchPattern = FindObjectOfType<MatchPattern>();
            StartCoroutine(CheckKeyTimeout());
        }

        void Update()
        {
            if (!_isKeyPressed && Input.anyKeyDown)
            {
                if (Input.GetKeyDown(assignedKey.ToString().ToLower()))
                {
                    _isKeyPressed = true;
                    _matchPattern.AddCount();
                    Destroy(gameObject);
                }
            }
        }

        IEnumerator CheckKeyTimeout()
        {
            yield return new WaitForSeconds(1f);

            if (!_isKeyPressed)
            {
                _matchPattern.Fail();
                Destroy(gameObject);
            }
        }

        public void SetSpawnPoint(Transform point)
        {
            _mySpawnPoint = point;
        }

        void OnDestroy()
        {
            if (_mySpawnPoint != null)
            {
                OnDestroyed?.Invoke(_mySpawnPoint);
            }
        }
    }
}
