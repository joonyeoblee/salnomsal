using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SeongIl
{
    public class ParrySequence : MonoBehaviour
    {
        public bool GameStart = false;
        public GameObject Jump;
        public Image Enemy;
        public Parry Parry;
        public Camera camera;
        public Image ParryZone;
        // 이미지 받아오기
        public Sprite EnemySprite;
        private void Start()
        {
            camera = Camera.main;
            Enemy.sprite = EnemySprite;
            StartCoroutine(Sequence());
            
        }
        
        public void GamePlay()
        {
            GameStart = true;
            
        }

        private IEnumerator Sequence()
        {
            Jump.SetActive(true);
            Enemy.enabled = false;
            GameStart = false;
            ParryZone.enabled = true;
            yield return new WaitForSeconds(1f);
            Parry.StartGame();
        }

        
    }
}