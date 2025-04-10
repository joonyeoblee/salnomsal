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

    public void ActivateOutlinable()
    {
        _outlinable.enabled = true;
    }

    public void DeactivateOutlinable()
    {
        _outlinable.enabled = false;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("clicked");
        CombatManager.Instance.SetTarget(GetComponent<ITargetable>());
    }
    public void ChangeOutlineColor(Color color)
    {
        _outlinable.OutlineParameters.Color = color;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CombatManager.Instance.ExitTarget();
    }
    void Reset()
    {
        if (GetComponent<Collider>() == null)
        {
            gameObject.AddComponent<CapsuleCollider2D>();
            gameObject.AddComponent<Outlinable>();
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        CombatManager.Instance.EnterTarget(this);
    }
}
