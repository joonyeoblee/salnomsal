using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UI_Selector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Image _image;
    Sprite _Orginimage;
    [SerializeField] Sprite _ChangeImage;

    void Start()
    {
        _image = GetComponent<Image>();
        _Orginimage = _image.sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _image.sprite = _ChangeImage;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _image.sprite = _Orginimage;
    }
}
