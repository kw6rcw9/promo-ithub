using System;
using System.Collections;
using System.Collections.Generic;
using InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CameraSystem
{
    public class ParallaxEffect : MonoBehaviour
    {
        [Tooltip("Насколько сильно слой смещается при движении камеры (0 — не двигается, 1 — двигается как камера)")]
        [Range(0f, 1f)]
        public float parallaxFactor = 0.5f;

        [Tooltip("Насколько плавно реагировать на движение камеры (0 = моментально, выше = медленнее)")]
        [Range(0f, 1f)]
        public float reactionSmoothing = 0.1f;

        private Transform cameraTransform;
        private Vector3 previousCameraPosition;
        private float smoothedDeltaY = 0f;
        private bool initialized = false;
        private RectTransform rectTransform;

        void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            cameraTransform = Camera.main.transform;
        }

        void LateUpdate()
        {
            if (gameObject.layer == 3 && rectTransform.localPosition.y <= -2000)
            {
                //Debug.LogError("SKY");
                return;
            }
                

            if (!initialized)
            {
                previousCameraPosition = cameraTransform.position;
                initialized = true;
                return;
            }

            Vector3 deltaMovement = cameraTransform.position - previousCameraPosition;

            // Сглаживаем только Y-смещение камеры (немного запаздываем)
            smoothedDeltaY = Mathf.Lerp(smoothedDeltaY, deltaMovement.y, 1f - reactionSmoothing);

            Vector3 parallaxMovement = new Vector3(0f, -smoothedDeltaY * parallaxFactor, 0f);
            transform.position += parallaxMovement;

            previousCameraPosition = cameraTransform.position;
        }
    }
}
