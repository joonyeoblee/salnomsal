using UnityEngine;
using UnityEngine.EventSystems;
[RequireComponent(typeof(Transform))]
public class TargetSelector : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("clicked");
        CombatManager.Instance.SetTarget(GetComponent<ITargetable>());
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse Enter");

        // 예: 하이라이트 처리
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Mouse Exit");

        // 예: 하이라이트 제거
    }
    void Reset()
    {
        if (GetComponent<Collider>() == null)
        {
            gameObject.AddComponent<CapsuleCollider2D>();
        }
    }
}
