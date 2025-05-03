using System;
using Equipment.RefactoringSlot;
using Portrait;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class SpawnPlayerR : MonoBehaviour, IPointerClickHandler
{
    public GameObject Portrait; // Portrait 프리팹
    public PortraitSO[] Portraits; // 가능한 초상화 리스트

    public CharacterSlotR[] CharacterSlot;

    public GameObject SpawnCanvas;

    void Awake()
    {
        Spawn();
    }
    public void Spawn()
    {
        for (var i = 0; i < CharacterSlot.Length; i++)
        {
            // 고유 ID 생성
            string uniqueID = Guid.NewGuid().ToString();

            // 초상화 프리팹 생성 및 위치 설정
            GameObject newItem = Instantiate(Portrait, transform);
            newItem.transform.localPosition = CharacterSlot[i].transform.localPosition;

            // Portrait 데이터 랜덤 선택
            PortraitSO selectedPortrait = Portraits[Random.Range(0, Portraits.Length)];

            // 컴포넌트 연결 및 초기화
            PortraitItem portraitItem = newItem.GetComponent<PortraitItem>();

            portraitItem.MyParent = CharacterSlot[i];
            portraitItem.IsInSlot = true;

            if (portraitItem != null)
            {
                portraitItem.portrait = selectedPortrait;
                portraitItem.Create(uniqueID);
                CharacterSlot[i].SetItem(portraitItem);

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
