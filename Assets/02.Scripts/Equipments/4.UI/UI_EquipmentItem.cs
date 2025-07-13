using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;

namespace Equipment
{
    public class UI_EquipmentItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,  IPointerClickHandler
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Image _border;

        private Vector3 _originPosition;
        private Canvas _canvas;
        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;
        [SerializeField] private UI_InventorySlot _inventorySlot;

        public EquipmentInstance EquipmentItemInstance => _equipmentInstance;
        private EquipmentInstance _equipmentInstance;

        public void Initialize(EquipmentInstance instance, UI_InventorySlot slot)
        {
            _inventorySlot = slot;
            _canvasGroup.blocksRaycasts = false;
            if (instance == null)
            {
                DeleteItem();
                return;
            }
            _equipmentInstance = instance;
            _canvasGroup.blocksRaycasts = true;
            LoadSprite(_equipmentInstance.IconAddress, _icon);
            LoadSprite(_equipmentInstance.BorderAddress, _border);
        }

        private void DeleteItem()
        {
            _canvasGroup.blocksRaycasts = false;
            _equipmentInstance = null;
            _icon.sprite = null;
            _border.sprite = null;
            _icon.color =  new Color(0, 0, 0, 0);
            _border.color =  new Color(0, 0, 0, 0);
        }

        private void Awake()
        {
            _canvas = GetComponentInParent<Canvas>();
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            _rectTransform = GetComponent<RectTransform>();
        }

        private void LoadSprite(string address, Image targetImage)
        {
            if (string.IsNullOrEmpty(address))
            {
                targetImage.sprite = null;
                _icon.color = new Color(0, 0, 0, 0);
                _border.color = new Color(0, 0, 0, 0);
                return;
            }

            Addressables.LoadAssetAsync<Sprite>(address).Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    targetImage.sprite = handle.Result;
                    _icon.color = Color.white;
                    _border.color = Color.white;
                }
                else
                {
                    Debug.LogError($"Load failed: {address}, status: {handle.Status}");
                }
            };
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _originPosition = transform.position;
            transform.SetParent(_canvas.transform); // Canvas 최상단으로 이동
            _canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = true;

            List<RaycastResult> results = new();
            PointerEventData pointerData = new(EventSystem.current)
            {
                position = Input.mousePosition
            };
            _canvas.GetComponent<GraphicRaycaster>().Raycast(pointerData, results);

            foreach (var hit in results)
            {
                if (hit.gameObject.TryGetComponent(out UI_InventorySlot targetSlot))
                {
                    if (targetSlot == _inventorySlot)
                    {
                        transform.SetParent(_inventorySlot.transform);
                        transform.position = _originPosition;
                        return;
                    }

                    var targetItem = targetSlot.GetComponentInChildren<UI_EquipmentItem>();

                    if (targetItem == null)
                    {
                        Debug.Log("[EndDrag] Dropped on empty slot. Move item and clear current.");

                        // 1. 대상 슬롯에 나의 데이터를 설정
                        targetSlot.Refresh(_equipmentInstance);

                        // 2. 현재 슬롯에 null 데이터 설정 (비우기)
                        _inventorySlot.Refresh(null);

                        // 3. 내 이미지도 숨김 처리
                        _icon.color = new Color(0, 0, 0, 0);
                        _border.color = new Color(0, 0, 0, 0);
                        _equipmentInstance = null;

                        // 4. 위치 복귀
                        transform.SetParent(_inventorySlot.transform);
                        transform.position = _originPosition;
                        
                        // Save 호출
                        EquipmentManager.Instance.Save();
                        return;
                    }
                    // 순수 데이터만 교환
                    var tempInstance = targetItem.EquipmentItemInstance;

                    targetItem.Initialize(_equipmentInstance, targetSlot);
                    Initialize(tempInstance, _inventorySlot);
                    
                    transform.SetParent(_inventorySlot.transform);
                    transform.position = _originPosition;
                    
                    // Save 호출
                    EquipmentManager.Instance.Save();
                    return;
                }
            }

            // 드롭 대상 없으면 원위치 복귀
            Debug.LogWarning("[EndDrag] No valid slot found. Returning to original position.");
            transform.SetParent(_inventorySlot.transform);
            transform.position = _originPosition;
            
            UI_Inventory.Instance.RebuildInstancesAndSave();
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                Debug.Log("이미지를 우클릭했습니다!");
                // 원하는 동작 실행
            }
        }
    }
}
