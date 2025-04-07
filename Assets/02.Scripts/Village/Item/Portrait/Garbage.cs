using UnityEngine;
using UnityEngine.EventSystems;

namespace Equipment
{
    public class Garbage : MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            Destroy(eventData.pointerDrag.gameObject);
        }
    }
}
