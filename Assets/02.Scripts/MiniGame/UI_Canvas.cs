using UnityEngine;

namespace Jun.MiniGame
{
    public class UI_Canvas : MonoBehaviour
    {
        Canvas _canvas;
        void Start()
        {
            _canvas = GetComponent<Canvas>();
            SetupCanvasCamera();
        }

        void SetupCanvasCamera()
        {
            Camera cam = Camera.main; // 혹은 원하는 카메라

            if (_canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                _canvas.worldCamera = cam;
            }
        }

    }
}