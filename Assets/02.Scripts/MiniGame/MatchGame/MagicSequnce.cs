
using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
namespace SeongIl
{


    public class MagicSequence : MonoBehaviour
    {
        public Image Book;
        public GameObject Enemy;
        public Image MainMagicCircle;
        public Image SubMagicCircle;
        public float ScaleValue;

        public IEnumerator StartSequence()
        {
            Enemy.transform.DOScale(ScaleValue, 0.5f).OnComplete(() =>
            {
                SubMagicCircle.DOColor(new Color(1,1,1, 1), 0.2f);
                MainMagicCircle.DOColor(new Color(1, 1, 1, 1), 0.5f);
            });
            yield return new WaitForSeconds(3f);
            Enemy.transform.DOScale(1f, 0.5f).OnComplete((() =>
            {
                Book.rectTransform.DOMove(new Vector3(0, -5f, 0) ,1f);
            }));
        }
        private void Update()
        {
            MainMagicCircle.rectTransform.Rotate(0,0, -5f * Time.deltaTime);
            SubMagicCircle.rectTransform.Rotate(0,0, 10f * Time.deltaTime);
        }

        public void Play()
        {
            StartCoroutine(StartSequence());
        }
    }
}