using UnityEngine;
using Equipment;

public class UI_InventorySlot : MonoBehaviour
{
    [SerializeField] private UI_EquipmentItem _item;

    public void Refresh(EquipmentInstance instance)
    {
        if (instance == null)
        {
            _item.gameObject.SetActive(false);
            return;
        }

        _item.gameObject.SetActive(true);
        _item.Initialize(instance);
    }
}