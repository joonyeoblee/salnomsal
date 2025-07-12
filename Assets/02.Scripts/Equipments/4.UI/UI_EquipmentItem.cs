using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Equipment;

public class UI_EquipmentItem : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private Image _border;

    private EquipmentInstance _instance;

    public void Initialize(EquipmentInstance instance)
    {
        _instance = instance;

        LoadSprite(_instance.IconAddress, _icon);
        LoadSprite(_instance.BorderAddress, _border);

        // 드래그 등 추가 가능
    }

    private void LoadSprite(string address, Image targetImage)
    {
        if (string.IsNullOrEmpty(address))
        {
            targetImage.sprite = null;
            return;
        }

        Addressables.LoadAssetAsync<Sprite>(address).Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
                targetImage.sprite = handle.Result;
            else
                Debug.LogError($"스프라이트 로드 실패: {address}");
        };
    }
}