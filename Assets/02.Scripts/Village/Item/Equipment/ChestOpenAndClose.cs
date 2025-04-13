using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
public class ChestOpenAndClose : MonoBehaviour
{
     public Tween Tween;

        [Header("상자")]
        public Image ChestImage;
        public Sprite OpenSprite;
        public Sprite ClosedSprite;
        
        [Header("스테이지")]
        public GameObject StageSelectButton;

        [Header("캐릭터 인벤토리")] 
        public GameObject CharactersInventoryPanel;
        public GameObject InventoryPanel;


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
            CharactersInventoryPanel.SetActive(true);
            ChestImage.sprite = OpenSprite;
            StageSelectButton.SetActive(false);
        }

        public void CloseChest()
        {
            Tween.Restart();
            CharactersInventoryPanel.SetActive(false);
            ChestImage.sprite = ClosedSprite;
            StageSelectButton.SetActive(true);
        }

        public void OpenInventoryPanel()
        {
            Tween.Restart();
            InventoryPanel.SetActive(true);
            ChestImage.sprite = ClosedSprite;
            CharactersInventoryPanel.SetActive(false);
        }
        
        public void CloseInventoryPanel()
        {
            Tween.Restart();
            InventoryPanel.SetActive(false);
            ChestImage.sprite = ClosedSprite;
            CharactersInventoryPanel.SetActive(true);
        }
}
