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


        private void Update()
        {
            if (GameStart)
            {
                StartCoroutine(Sequence());
            }
            
        }
        public void GamePlay()
        {
            GameStart = true;
            
        }

        private IEnumerator Sequence()
        {
            Refresh();
            Jump.SetActive(true);
            Enemy.enabled = false;
            GameStart = false;
            yield return new WaitForSeconds(2f);
            Parry.GameStart = true;
        }

        private void Refresh()
        {
            if (Enemy.enabled == false)
            {
                Jump.SetActive(false);
                Enemy.enabled = true;
            }
        }
        
        
    }
}