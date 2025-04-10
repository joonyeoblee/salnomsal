using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace SeongIl
{
    public class AvoidSequence : MonoBehaviour
    {
        
        // public Camera MyCamera;
        public Image Enemy;
        public Image BackGround;

        public GameObject[] Effect;
        // public GameObject Player;
        
        private void Start()
        {
            StartSequence();
            
        }
    
        private void StartSequence()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(Enemy.rectTransform.DOAnchorPos(new Vector2(500, 0), 1f).SetEase(Ease.OutCubic));

            sequence.AppendInterval(1f);
            StartCoroutine(EffectOn());
            sequence.Append(Enemy.rectTransform.DOAnchorPos(new Vector2(1400, 0), 1f).SetEase(Ease.OutCubic));
            sequence.Join(BackGround.DOColor(new Color(0f, 0f, 0f, 0f),0.7f).SetEase(Ease.OutCubic));
            sequence.Join(BackGround.rectTransform.DOScale(new Vector3(0,1f,1f),1f).SetEase(Ease.OutCubic));
        }

        private IEnumerator EffectOn()
        {
            yield return new WaitForSeconds(1f);
            
            for (int i = 0; i < Effect.Length; i++)
            {
                Effect[i].SetActive(true);
                yield return new WaitForSeconds(0.1f);
            }
            
            GameStart();
            
        }

        private void GameStart()
        {
            
        }
    }
}