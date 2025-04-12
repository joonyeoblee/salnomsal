using System;
using DG.Tweening;
using SeongIl;
using UnityEngine;

namespace SeongIl
{
    public class AvoidBounce : MonoBehaviour
    {
        void Update()
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                transform.DOKill(); // 이전 Tween 종료 (중복 방지)
                transform.DOScale(1.2f, 0.1f)
                    .SetEase(Ease.OutBounce)
                    .OnComplete(() =>
                    {
                        transform.DOScale(1f, 0.1f)
                            .SetEase(Ease.OutBounce);
                    });
            }
        }
    }
}