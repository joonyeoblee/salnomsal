using UnityEngine;
using UnityEngine.EventSystems;
[RequireComponent(typeof(Transform))]
public class TargetSelector : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("clicked");
        CombatManager.Instance.SetTarget(GetComponent<ITargetable>());
    }

    void Reset()
    {
        if (GetComponent<Collider>() == null)
        {
            gameObject.AddComponent<CapsuleCollider2D>();
        }
    }
}
