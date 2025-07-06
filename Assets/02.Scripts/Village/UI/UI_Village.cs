using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Son
{
    public class UI_Village : MonoBehaviour
    {
        private Tween Tween;

        [Header("캐릭터")]
        public GameObject PortraitPanel;
        public GameObject SpawnPanel;

        [Header("스테이지")]
        public GameObject StageSelectButton;
        public GameObject StageSelectPanel;
        public GameObject ExitStageSelectPanelButton;
        


        public void Awake()
        {
            transform.localScale = Vector3.one;
            Tween = transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.1f)
                .SetEase(Ease.OutQuad)
                .SetLoops(2, LoopType.Yoyo)
                .SetAutoKill(false)
                .Pause();
        }
        private void Start()
        {
            CloseAll();
        }

        public void OpenPortraitPanel()
        {
            Tween.Restart();
            PortraitPanel.SetActive(true);
        }

        public void ClosePortrait()
        {
            Tween.Restart();
            PortraitPanel.SetActive(false);

        }

        public void CloseSpawn()
        {
            Tween.Restart();
            SpawnPanel.SetActive(false);
        }

        public void CloseAll()
        {
            // CloseChest();
            ClosePortrait();
            CloseSpawn();

        }
        public void ActiveStageSelectPanel()
        {
            StageSelectButton.SetActive(false);
            StageSelectPanel.SetActive(true);
            ExitStageSelectPanelButton.SetActive(true);
            CloseAll();
        }

        public void DeActiveStageSelectPanel()
        {
            StageSelectButton.SetActive(true);
            StageSelectPanel.SetActive(false);
            ExitStageSelectPanelButton.SetActive(false);
        }


        //public void ExitGame()
        //{
        //    Tween.Restart();

        //    Application.Quit();
        //}
    }

}