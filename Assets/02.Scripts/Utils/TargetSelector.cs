using EPOOutline;
using UnityEngine;
using UnityEngine.EventSystems;
[RequireComponent(typeof(Transform))]
public class TargetSelector : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    Outlinable _outlinable;

    void Start()
    {
        _outlinable = GetComponent<Outlinable>();
        _outlinable.enabled = false;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("clicked");
        CombatManager.Instance.SetTarget(GetComponent<ITargetable>());
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _outlinable.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _outlinable.enabled = false;
    }
    void Reset()
    {
        if (GetComponent<Collider>() == null)
        {
            gameObject.AddComponent<CapsuleCollider2D>();
            gameObject.AddComponent<Outlinable>();
        }
    }
}
