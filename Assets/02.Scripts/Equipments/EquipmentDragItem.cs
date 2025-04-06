using UnityEngine;

namespace Equipment
{
    public class EquipmentDragItem : DragItem
    {
        public EquipmentInstance EquipmentInstance;

        public void Initialize(EquipmentInstance equipmentInstance)
        {
            EquipmentInstance = equipmentInstance;
            GetComponent<UnityEngine.UI.Image>().sprite = equipmentInstance.Template.Icon;
        }
    }
}