using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
// 추가 필요

public class ItemDragger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private bool _isDragging = false;
    private Vector3 _dragOffset;
    private Vector3 _originPosition;
    private Camera _mainCamera;
    private void Start()
    {
        _originPosition = transform.position;
        _mainCamera = Camera.main;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        _isDragging = true;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 worldPos = _mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, _mainCamera.nearClipPlane));

        _dragOffset = transform.position - worldPos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_isDragging) return;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 worldPos = _mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, _mainCamera.nearClipPlane));

        transform.position = worldPos + _dragOffset;
    }


    // 마우스 떼었을 때
    public void OnPointerUp(PointerEventData eventData)
    {

        var hits = Physics2D.OverlapPointAll(transform.position);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("PlayerSlot"))
            {
                var slotCollider = hit.GetComponent<Collider2D>();

                // 원위치로 되돌리기
                transform.position = _originPosition;
                return;
            }
        }

        // 원위치로 되돌리기
        transform.position = _originPosition;
    }
}
