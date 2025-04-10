

using DG.Tweening;
using Jun.MiniGame;
using UnityEngine;
using UnityEngine.UI;
namespace SeongIl
{


    public class MagicSequence : MonoBehaviour
    {
        public GameObject CircleVFX; 
        // 에너미 스프라이트 렌더러 받아오기
        public Sprite EnemyRenderer;
        public Image Transition;
        public Camera Camera;
        public Image Book;
        public GameObject Enemy;
        public Image MainMagicCircle;
        public Image SubMagicCircle;
        public float ScaleValue;
        private void Start()
        {

            Camera = Camera.main;
            //때긴놈 spriterenderer 입히기
           // EnemyRenderer = new SpriteRenderer()
           
           Enemy.GetComponent<SpriteRenderer>().sprite = EnemyRenderer;
           Transition.DOColor(new Color(0, 0, 0, 0), 1f).OnComplete(() =>
           {
               GamePlay();
           });
           
        }


        // 마법 연출
        public void StartSequence()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(Camera.DOOrthoSize(3f, ScaleValue).SetEase(Ease.OutCubic));
            sequence.Join(Camera.transform.DOMove(new Vector3(0, -2f, -10f), ScaleValue).SetEase(Ease.OutCubic));
            //
            sequence.AppendInterval(0.2f);

            sequence.Append(Camera.transform.DOMove(new Vector3(0, 2f, -10f), ScaleValue).SetEase(Ease.OutCubic));
            
            sequence.Append( SubMagicCircle.DOColor(new Color(1,1,1, 1), 0.2f));
            sequence.Append(MainMagicCircle.DOColor(new Color(1,1,1, 1), 0.2f));
            
            sequence.AppendInterval(0.2f);
            
            sequence.Append(Camera.DOOrthoSize(5f, ScaleValue).SetEase(Ease.OutCubic));
            sequence.Join(SubMagicCircle.rectTransform.DOScale(10f, ScaleValue).SetEase(Ease.OutCubic));
            sequence.Join(MainMagicCircle.rectTransform.DOScale(10f, ScaleValue).SetEase(Ease.OutCubic));
            sequence.Join(SubMagicCircle.rectTransform.DOMoveY( 1f, ScaleValue).SetEase(Ease.OutCubic));
            sequence.Join(MainMagicCircle.rectTransform.DOMoveY( 1f, ScaleValue).SetEase(Ease.OutCubic));
            sequence.Join(Camera.transform.DOMove(new Vector3(0, 0f, -10f), ScaleValue).SetEase(Ease.OutCubic));
            sequence.Join(Book.rectTransform.DOMove(new Vector3(0, -5f, 0) ,1f));
            sequence.OnComplete((() =>
            {
                CircleVFX.SetActive(true);
            }));

        }
        private void Update()
        {
            MainMagicCircle.rectTransform.Rotate(0,0, -5f * Time.deltaTime);
            SubMagicCircle.rectTransform.Rotate(0,0, 10f * Time.deltaTime);
        }

        public void GamePlay()
        { 
            StartSequence();
            
            MatchPattern magic = GetComponent<MatchPattern>();
            StartCoroutine(magic.GameStart());
        }
    }
}