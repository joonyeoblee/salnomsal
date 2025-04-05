using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Jun.MiniGame
{
    public class Magic : MonoBehaviour
    {
        public Image FillImage;
        public char assignedKey;
        private bool _isKeyPressed = false;
        private MatchPattern _matchPattern;

        void Start()
        {
            
            FillImage = GetComponent<Image>();
            _matchPattern = GetComponentInParent<MatchPattern>();
            _matchPattern.RegisterMagic(this);
            FillImage.fillAmount = 0;
            StartCoroutine(CheckKeyTimeout());
            StartCoroutine(FillImageOverTime());
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

        IEnumerator CheckKeyTimeout()
        {
            yield return new WaitForSeconds(1f);

            if (!_isKeyPressed)
            {
                _matchPattern.Fail();
                _matchPattern.UnregisterMagic(this);
                Destroy(gameObject);
            }
        }

        IEnumerator FillImageOverTime()
        {
            float elapsed = 0f;
            float duration = 1f;

            while (elapsed < duration)
            {
                if (_isKeyPressed) yield break;

                elapsed += Time.deltaTime;
                float fill = Mathf.Clamp01(elapsed / duration);

                if (FillImage != null)
                    FillImage.fillAmount = fill;

                yield return null;
            }

            // 시간 초과되면 실패 처리 (중복 방지용 안전 장치)
            if (!_isKeyPressed)
            {
                _matchPattern.Fail();
                _matchPattern.UnregisterMagic(this);
                Destroy(gameObject);
            }
        }
    }

    // void OnDestroy()
    // {
    //     if (_matchPattern != null)
    //     {
    //         _matchPattern.UnregisterMagic(this);
    //     }
    // }
}
