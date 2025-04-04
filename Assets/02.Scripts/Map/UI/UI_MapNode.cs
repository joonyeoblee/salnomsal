using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Jun.Map
{
    public class UI_MapNode : MonoBehaviour
    {
        public Image iconImage;
        public TMP_Text label;

        public void Init(Vector2 position, string labelText, Color color)
        {
            GetComponent<RectTransform>().anchoredPosition = position;
            iconImage.color = color;
            label.text = labelText;
        }
    }
}
