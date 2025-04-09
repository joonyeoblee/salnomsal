using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

// 인터페이스
public class UI_TouchBounce : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // 마우스를 터치하면 크기를 변경(키우거나 늘리거나)
    public Vector3 StartScale;
    public float EndScale = 1.2f;
    public float Duration = 0.1f;

    RectTransform _rectTransform;
    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    void OnDisable()
    {
        _rectTransform.localScale = StartScale;
    }

    void Start()
    {
        StartScale = _rectTransform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _rectTransform.DOScale(EndScale, Duration).SetUpdate(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _rectTransform.DOScale(StartScale, Duration).SetUpdate(true);
    }
}
