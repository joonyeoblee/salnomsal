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
            this.transform.DOScaleY(0f, ShootingTime).SetEase(Ease.OutCirc).OnComplete(() =>
            {
                Destroy(this.gameObject);
            });
        }

    }
}