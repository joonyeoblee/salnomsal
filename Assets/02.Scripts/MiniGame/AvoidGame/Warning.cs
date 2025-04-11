using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace SeongIl
{


    public class Warning : MonoBehaviour
    {
        public float ShootingTime = 2f;

        private void Start()
        {
            Animation();
        }

        private void Animation()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(this.transform.DOScaleY(0f, ShootingTime).SetEase(Ease.OutCirc));
            sequence.Join(this.transform.DOScaleX(20f, ShootingTime).SetEase(Ease.InCirc));
            sequence.OnComplete(() =>
            {
                Destroy(this.gameObject);
            });
            
        }

    }
}