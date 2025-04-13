using UnityEngine;
using DG.Tweening;

namespace SeongIl
{
    

public class ParryBounceUI : MonoBehaviour
{
    private RectTransform _rectTransform;
    private Parry isBounce;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }
    private void Start()
    {
        isBounce = GetComponent<Parry>();
        
    }
    private void Update()
    {
        if (isBounce.LastParry)
        {
            return;
        }
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
}