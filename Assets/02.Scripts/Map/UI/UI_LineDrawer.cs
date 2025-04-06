using UnityEngine;

namespace Jun.Map
{
    public class UI_LineDrawer : MonoBehaviour
    {
        public void DrawLine(Vector2 start, Vector2 end)
        {
            RectTransform rect = GetComponent<RectTransform>();
            Vector2 diff = end - start;
            float distance = diff.magnitude;

            // 길이 설정
            rect.sizeDelta = new Vector2(distance, 4); // 선 두께 4

            // 시작점 위치로 이동
            rect.anchoredPosition = start;

            // 각도 회전 (0, 0.5 기준으로)
            float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            rect.localRotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
