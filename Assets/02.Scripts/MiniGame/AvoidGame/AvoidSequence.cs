using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace SeongIl
{
    public class AvoidSequence : MonoBehaviour
    {
        
        public Camera MyCamera;
        public Canvas MyCanvas;
        public Image Enemy;
        public GameObject Player;
        
        
        private void Start()
        {
            MyCamera = MyCamera.GetComponent<Camera>(); 
            // sprite 불러오기
             // Player.GetComponent<SpriteRenderer>().sprite = ;
            //   Enemy.sprite = MySprite;
            // sprite 불러오기
            
            
        }
    
        private void Sequence()
        {
            Sequence sequence = DOTween.Sequence();
            
        }
    }
}