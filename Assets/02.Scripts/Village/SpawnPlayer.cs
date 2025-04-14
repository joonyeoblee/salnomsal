using System;
using Portrait;
using Spawn;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class SpawnPlayer : MonoBehaviour, IPointerClickHandler
{
    public GameObject portrait; // Portrait 프리팹
    public PortraitSO[] portraits; // 가능한 초상화 리스트

    public SpawnSlot[] spawnSlot;

    public GameObject SpawnCanvas;

    void Awake()
    {
        Spawn();
    }
    public void Spawn()
    {
        for (int i = 0; i < spawnSlot.Length; i++)
        {
            // 고유 ID 생성
            string uniqueID = Guid.NewGuid().ToString();

            // 초상화 프리팹 생성 및 위치 설정
            GameObject newItem = Instantiate(portrait, transform);
            newItem.transform.localPosition = spawnSlot[i].transform.localPosition;

            // Portrait 데이터 랜덤 선택
            PortraitSO selectedPortrait = portraits[Random.Range(0, portraits.Length)];

            // 컴포넌트 연결 및 초기화
            PortraitItem portraitItem = newItem.GetComponent<PortraitItem>();

            // portraitItem.MyParent = spawnSlot[i];
            // portraitItem.IsInSlot = true;

            if (portraitItem != null)
            {
                portraitItem.portrait = selectedPortrait;
                portraitItem.Init(uniqueID);
                spawnSlot[i].SetItem(portraitItem);

                Debug.Log($"새 플레이어 소환됨 | ID: {uniqueID} | Portrait: {selectedPortrait.name}");
            } else
            {
                Debug.LogWarning("PortraitItem 컴포넌트를 찾을 수 없습니다.");
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SpawnCanvas.SetActive(true);
    }
}
