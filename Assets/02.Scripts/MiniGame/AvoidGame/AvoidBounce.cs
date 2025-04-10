using DG.Tweening;
using UnityEngine;

public class AvoidBounce : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.DOKill(); // 이전 Tween 종료 (중복 방지)
            transform.DOScale(1.4f, 0.1f)
                .SetEase(Ease.OutBounce)
                .OnComplete(() =>
                {
                    transform.DOScale(1f, 0.1f)
                        .SetEase(Ease.OutBounce);
                });
        }
    }
}
