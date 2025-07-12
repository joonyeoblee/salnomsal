using UnityEngine;
using UnityEngine.EventSystems;

namespace Equipment
{
    public class EquipmentDragItem : DragItem, IPointerEnterHandler, IPointerExitHandler
    {
        public EquipmentInstance EquipmentInstance;

        public void Initialize(EquipmentInstance equipmentInstance)
        {
          
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (UI_ItemTooltip.Instance == null)
            {
                Debug.LogWarning("UI_ItemTooltip.Instance가 null입니다!");
            }
            else
            {
                UI_ItemTooltip.Instance.Show(EquipmentInstance);
            }
            
            UI_ItemTooltip.Instance.Show(EquipmentInstance);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (UI_ItemTooltip.Instance == null)
            {
                Debug.LogWarning("UI_ItemTooltip.Instance가 null입니다!");
            }
            else
            {
                UI_ItemTooltip.Instance.Show(EquipmentInstance);
            }
            UI_ItemTooltip.Instance.Hide();
        }
    }
}