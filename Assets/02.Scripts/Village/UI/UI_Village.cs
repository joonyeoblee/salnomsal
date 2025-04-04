using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

namespace Son
{
    public class UI_Village : MonoBehaviour
    {
        public Tween Tween;

        [Header("상자")]
        public GameObject ChestInventoryPanel;
        public Image ChestImage;
        public Sprite OpenSprite;
        public Sprite ClosedSprite;

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

        public void OpenChest()
        {
            Tween.Restart();
            ChestInventoryPanel.SetActive(true);
            ChestImage.sprite = OpenSprite;
            ExitStageSelectPanelButton.SetActive(true);
            StageSelectButton.SetActive(false);
        }

        public void CloseChest()
        {
            Tween.Restart();
            ChestInventoryPanel.SetActive(false);
            ChestImage.sprite = ClosedSprite;
            ExitStageSelectPanelButton.SetActive(false);
            StageSelectButton.SetActive(true);
        }

        public void ActiveStageSelectPanel()
        {
            StageSelectButton.SetActive(false);
            StageSelectPanel.SetActive(true);
            ExitStageSelectPanelButton.SetActive(true);
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