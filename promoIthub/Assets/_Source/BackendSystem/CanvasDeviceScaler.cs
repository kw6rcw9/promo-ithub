using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

namespace BackendSystem
{
    public class CanvasDeviceScaler : MonoBehaviour
    {
        public CanvasScaler canvasScaler;

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern int IsMobileDevice();
#endif

        void Start()
        {
            if (canvasScaler == null)
                canvasScaler = GetComponent<CanvasScaler>();

            bool isMobile = false;

#if UNITY_WEBGL && !UNITY_EDITOR
        isMobile = IsMobileDevice() == 1;
#else
            isMobile = Application.isMobilePlatform;
#endif

            if (isMobile)
            {
                canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                canvasScaler.referenceResolution = new Vector2(540, 1140);
                canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
            }
            else
            {
                canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
            }
        }
    }
}