using Jun.MiniGame;
using MoreMountains.Tools;
using UnityEngine;

namespace Jun.MiniGame
{

    public class Magic : MonoBehaviour
    {
        public MMProgressBar ProgressBar;
        public char assignedKey;
        private bool _isKeyPressed = false;
        private MatchPattern _matchPattern;

        public float MaxTime;
        float _currentTime = 0f;

        void Start()
        {
            _matchPattern = GetComponentInParent<MatchPattern>();
            _matchPattern.RegisterMagic(this);
            ProgressBar = GetComponent<MMProgressBar>();
            ProgressBar.FillMode = MMProgressBar.FillModes.FillAmount;
            ProgressBar.InitialFillValue = 0f;
        }

        private void Update()
        {
            _currentTime += Time.deltaTime;

            // MMProgressBar 업데이트
            if (ProgressBar != null)
            {
                ProgressBar.UpdateBar(_currentTime,0,MaxTime);
            }

            if (_currentTime >= MaxTime)
            {
                _isKeyPressed = false;
                _matchPattern.UnregisterMagic(this);
                _matchPattern.Fail();
            }
        }

        public void TryResolve()
        {
            if (_isKeyPressed) return;

            _isKeyPressed = true;
            _matchPattern.UnregisterMagic(this);
            _matchPattern.AddCount();
            Debug.Log("성공 매직");
            Destroy(gameObject);
        }
    }
}