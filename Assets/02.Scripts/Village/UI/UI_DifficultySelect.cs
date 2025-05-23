﻿using System.Collections.Generic;
using DG.Tweening;
using Jun;
using UnityEngine;
using UnityEngine.UI;

namespace Son
{
    public class UI_DifficultySelect : MonoBehaviour
    {
        public Tween Tween;

        [Header("스테이지 선택")]
        public List<RectTransform> DifficultPortal;
        public int CurrentIndex = 0;
        public float moveDistance = 300f;
        public float moveDuration = 0.3f;
        public GameObject LeftButton;
        public GameObject RightButton;

        void OnEnable()
        {
            UpdatePositions();
        }
        public void MoveLeft()
        {
            if (CurrentIndex > 0)
            {
                CurrentIndex--;
                UpdatePositions();
            }
        }

        public void MoveRight()
        {
            if (CurrentIndex < DifficultPortal.Count - 1)
            {
                CurrentIndex++;
                UpdatePositions();
            }
        }

        void UpdatePositions()
        {
            for (int i = 0; i < DifficultPortal.Count; i++)
            {
                float baseDistance = moveDistance;
                float distanceFromCenter = Mathf.Abs(i - CurrentIndex);
                float distanceFactor = Mathf.Pow(0.8f, distanceFromCenter); // 점점 작아짐
                
                float targetX = (i - CurrentIndex) * baseDistance * distanceFactor;
                DifficultPortal[i].DOAnchorPosX(targetX, moveDuration).SetEase(Ease.OutBack);

                // float targetScale = (i == CurrentIndex) ? 1.0f : 0.6f;
                float scale = Mathf.Lerp(1.0f, 0.6f, distanceFromCenter / 2f);
                DifficultPortal[i].DOScale(Vector3.one * scale, moveDuration).SetEase(Ease.OutBack);

                // 선택된 포탈만 버튼 활성화
                Button btn = DifficultPortal[i].GetComponent<Button>();
                if (btn != null)
                {
                    btn.interactable = (i == CurrentIndex);
                }
            }
            LeftButton.SetActive(CurrentIndex > 0);
            RightButton.SetActive(CurrentIndex < DifficultPortal.Count - 1);
        }


        public void OnMapSelected()
        {
            MiniGameScenesManager.Instance.ChangeScene(SceneIndex.StartMapScene);
        }

    }
}