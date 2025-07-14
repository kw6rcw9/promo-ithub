using Cinemachine;
using UnityEngine;

namespace CameraSystem
{
    public class LockCameraX : MonoBehaviour
    {
        public float fixedX = 0f;

        void LateUpdate()
        {
            var cam = GetComponent<CinemachineVirtualCamera>();
            if (cam != null)
            {
                var pos = cam.transform.position;
                cam.transform.position = new Vector3(fixedX, pos.y, pos.z);
            }
        }
    }
}
