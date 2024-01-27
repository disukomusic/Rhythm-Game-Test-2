using System;
using UnityEngine;

namespace Menu
{
    public class RotateOptions : MonoBehaviour
    {
        [SerializeField] private float currentRotationSpeed;

        public float startRotSpeed = 100f;
        public float endRotSpeed = 5f;

        private float t;

        private void Update()
        {
            if (currentRotationSpeed > endRotSpeed)
            {
                t += Time.deltaTime;
                float normalizedTime = Mathf.Clamp01(t / (currentRotationSpeed - endRotSpeed));
                float easedTime = Mathf.SmoothStep(0f, 1f, normalizedTime);
                currentRotationSpeed = Mathf.Lerp(startRotSpeed, endRotSpeed, easedTime);
            }
            else
            {
                currentRotationSpeed = endRotSpeed;
            }

            transform.Rotate(Vector3.forward, currentRotationSpeed * Time.deltaTime);
        }
    }
}