using DG.Tweening;
using Jun;
using UnityEngine;
using UnityEngine.UI;

namespace TitleMenu
{
    public class UI_TitleMenu : MonoBehaviour
    {
        public string LevelToLoad;

        public Tween Tween;

        public Image FadeImage;

        public void Awake()
        {
            transform.localScale = Vector3.one;

            LevelToLoad = "SampleScene";
            Tween = transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.1f)
                .SetEase(Ease.OutQuad)
                .SetLoops(2, LoopType.Yoyo)
                .SetAutoKill(false)
                .Pause();
        }

        public void StartGame()
        {
            Tween.Restart();
            FadeImage.gameObject.SetActive(true);

            FadeImage.color = new Color(0, 0, 0, 0); // 시작은 투명
            FadeImage.DOFade(1f, 1f) // 1초간 서서히 검게
                .SetEase(Ease.OutQuad)
                .OnComplete(() => { MiniGameScenesManager.Instance.ChangeScene(SceneIndex.Village); });
        }

        public void CancelStageSelect()
        {
            Tween.Restart();
        }


        public void ExitGame()
        {
            Tween.Restart();
            Application.Quit();
        }
    }
}