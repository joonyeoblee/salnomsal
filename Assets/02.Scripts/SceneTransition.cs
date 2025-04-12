using System;
using System.Collections;
using DG.Tweening;
using Jun.Map;
using UnityEngine;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;

namespace SeongIl
{
    public class SceneTransition : MonoBehaviour
    {
        public Action IsTransition;
        public Image Fade;
        public Image Loading;
        public Image Hit;

        public GameObject StartBattle;
        private void Start()
        {
            IsTransition += BasicTranstition;

            
            
        }

        public void BasicTranstition()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append( Fade.DOColor(new Color(0f, 0f, 0f, 1f), 0).SetEase(Ease.OutCubic));
            sequence.Join(Loading.GetComponent<SpriteRenderer>().DOFade(1f, 1f).SetEase(Ease.OutCubic));
            sequence.Join(Loading.rectTransform.DOPivot(new Vector2(331f, 182f), 1f));
            sequence.AppendInterval(0.5f);
            sequence.Append(Fade.DOColor(new Color(0f, 0f, 0f, 0f), 1f).SetEase(Ease.InCubic));
            sequence.Join(Loading.GetComponent<SpriteRenderer>().DOFade(0f, 0.7f).SetEase(Ease.InCubic)).OnComplete(() => { MapManager.Instance.OnMapNodeChanged += NodeTranstition; });
            Debug.Log("씬 전환");
        }

        public void NodeTranstition()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(Fade.DOColor(new Color(0f, 0f, 0f, 1f), 0).SetEase(Ease.OutCubic));
            sequence.Join(Loading.GetComponent<SpriteRenderer>().DOFade(1f, 0.3f).SetEase(Ease.OutCubic));
            sequence.Join(Loading.rectTransform.DOPivot(new Vector2(331f, 182f), 0.3f));

            // sequence.AppendInterval(f);
            sequence.Append(Fade.DOColor(new Color(0f, 0f, 0f, 0f), 0.7f).SetEase(Ease.InCubic));
            sequence.Join(Loading.GetComponent<SpriteRenderer>().DOFade(0f, 0.2f).SetEase(Ease.InCubic)).OnComplete(() => { ActiveBattle(); });

            Debug.Log("씬 전환");
        }
        public IEnumerator MiniGameTransition()
        {
            Fade.DOColor(new Color(1,0,0, 0.15f), 0f);
            Hit.DOFade(0.8f, 0.15f);
            
            yield return new WaitForSeconds(0.2f);
            
            Hit.DOFade(0f, 0.2f);
            
            Fade.DOColor(new Color(1,0,0, 0f), 0.2f);

        }

        void ActiveBattle()
        {
            StartBattle.SetActive(true);

        }

        public void DeActiveBattle()
        {
            StartBattle.SetActive(false);
        }
        
    }
}

