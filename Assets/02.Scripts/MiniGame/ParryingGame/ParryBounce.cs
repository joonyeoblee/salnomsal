using UnityEngine;
using DG.Tweening;

public class ParryBounceUI : MonoBehaviour
{
    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rectTransform.DOKill(); // 이전 Tween 종료 (중복 방지)
            _rectTransform.DOScale(1.4f, 0.1f)
                .SetEase(Ease.OutBounce)
                .OnComplete(() =>
                {
                    _rectTransform.DOScale(1.3f, 0.1f)
                        .SetEase(Ease.OutBounce);
                });
        }
    }
}