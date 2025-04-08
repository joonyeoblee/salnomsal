using System;
using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace SeongIl
{
    public class SceneTransition : MonoBehaviour
    {
        public Action IsTransition;
        public Image Fade;
        public Image Loading;
        private void Start()
        {
            IsTransition += BasicTranstition;
        }

        public void BasicTranstition()
        {
            Fade.DOColor(new Color(0f, 0f, 0f, 1f), 0).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                
                Fade.DOColor(new Color(0f, 0f, 0f, 0f), 1f).SetEase(Ease.InCubic);
            });
            
            Debug.Log("씬 전환");
        }

        public IEnumerator MiniGameTransition()
        {
            Fade.DOColor(new Color(0f, 0f, 0f, 1f), 2f);
            yield return new WaitForSeconds(3f);
            
        }
        
    }
}

