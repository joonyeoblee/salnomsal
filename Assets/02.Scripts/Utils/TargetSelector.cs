using UnityEngine;
using UnityEngine.EventSystems;

public class TargetSelector : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        // UI에 클릭한 타겟의 정보 표시
        // CombatManager에 타겟 정보 전달
        Debug.Log("clicked");
        CombatManager.Instance.SetTarget(gameObject.GetComponent<ITargetable>());
    }
}
