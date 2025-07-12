using Equipment;
using UnityEngine;
using UnityEngine.UI;

public class UI_InventorySlot : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private Image _borderImage;

    public void Refresh(EquipmentInstance equipmentInstance)
    {
        if (equipmentInstance == null)
        {
            Debug.LogWarning("Empty equipment data.");
            _iconImage.sprite = null;
            _borderImage.color = Color.clear;
            return;
        }

     

        SetBorderColor(equipmentInstance.Rarity);
    }

    private void SetBorderColor(Rarity rarity)
    {
        _borderImage.color = rarity switch
        {
            Rarity.Common    => Color.gray,
            Rarity.Rare      => new Color(0.2f, 0.6f, 1f),
            Rarity.Epic      => new Color(0.6f, 0f, 1f),
            Rarity.Legendary => new Color(1f, 0.8f, 0f),
            _                => Color.white
        };
    }
}