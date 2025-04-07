using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Equipment
{
    [Serializable]
    public class SlotItemData
    {
        public string id;
    }
    public class Slot : MonoBehaviour, IDropHandler
    {
        [SerializeField] RectTransform _draggedSlot;
        [SerializeField] public GameObject currentEquipment;
        [SerializeField] Canvas canvas;
        [SerializeField] GameObject _itemPrefab;
        SlotItemData _slotItemData;
        string SaveKey;
        void Start()
        {
            SaveKey = "ItemSlot_" + transform.GetSiblingIndex();
            Load();
        }
        public void SetItem(DragItem equipment)
        {
            currentEquipment = equipment.gameObject;
            currentEquipment.transform.SetParent(transform);
            currentEquipment.transform.localPosition = _draggedSlot.localPosition;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (currentEquipment != null)
            {
                return;
            }

            DragItem dragItem = eventData.pointerDrag.GetComponent<DragItem>();
            dragItem.IsInSlot = true;
            dragItem.MyParent = this;
            if (dragItem != null)
            {
                SetItem(dragItem);
                Save();
                // Destroy(dragItem.gameObject);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (currentEquipment == null)
            {
                return;
            }

            // 드래그 오브젝트 가져오기
            DragItem drag = currentEquipment.GetComponent<DragItem>();
            if (drag == null)
            {
                return;
            }

            // 슬롯에서 제거 (자식에서 분리)
            currentEquipment.transform.SetParent(canvas.transform);
            Debug.Log("드래그 시작 전 currentEquipment: " + currentEquipment?.name);
            currentEquipment = null;
            Debug.Log("드래그 후 currentEquipment: " + currentEquipment?.name);

            // 드래그 시작
            // drag.StartManualDrag();


        }


        void Save()
        {
            if (currentEquipment != null)
            {
                // DragItem에서 ID 꺼내기
                EquipmentGenerator item = currentEquipment.GetComponent<EquipmentGenerator>();
                if (item != null && item.ItemId != null)
                {
                    SlotItemData slotItemData = new SlotItemData();
                    slotItemData.id = item.ItemId;
                    string data = JsonUtility.ToJson(slotItemData);
                    PlayerPrefs.SetString(SaveKey, data);
                    PlayerPrefs.Save();
                    Debug.Log($"[Slot {SaveKey}] 저장됨: {data}");
                }
            }
        }

        void Load()
        {
            if (PlayerPrefs.HasKey(SaveKey))
            {
                string json = PlayerPrefs.GetString(SaveKey);
                _slotItemData = JsonUtility.FromJson<SlotItemData>(json);
                Debug.Log($"[Slot {SaveKey}] 불러옴: {_slotItemData.id}");

                // 아이템 프리팹을 인스턴스화해서 슬롯에 배치
                GameObject newItem = Instantiate(_itemPrefab, transform);
                newItem.transform.localPosition = _draggedSlot.localPosition;

                // ID 설정
                EquipmentGenerator ItemSetId = newItem.GetComponent<EquipmentGenerator>();
                DragItem dragItem = ItemSetId.GetComponent<DragItem>();
                if (dragItem != null)
                {
                    ItemSetId.Init(_slotItemData.id);
                    dragItem.MyParent = this;
                    dragItem.IsInSlot = true;
                }

                currentEquipment = newItem;
            }
        }

        public void DeleteItem()
        {
            if (currentEquipment != null)
            {
                currentEquipment = null; // 참조도 제거
                Debug.Log($"[Slot {SaveKey}] 아이템 제거됨.");
            }

            if (PlayerPrefs.HasKey(SaveKey))
            {
                PlayerPrefs.DeleteKey(SaveKey); // 저장된 데이터도 제거
                PlayerPrefs.Save();
                Debug.Log($"[Slot {SaveKey}] 저장 데이터 삭제됨.");
            }
        }
    }
}
