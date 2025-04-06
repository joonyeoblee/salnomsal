using UnityEngine;
using UnityEngine.EventSystems;

namespace Equipment
{
    public class Slot : MonoBehaviour, IDropHandler
    {
        [SerializeField] RectTransform _draggedSlot;
        [SerializeField] public GameObject currentEquipment;
        [SerializeField] Canvas canvas;

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
    }
}
