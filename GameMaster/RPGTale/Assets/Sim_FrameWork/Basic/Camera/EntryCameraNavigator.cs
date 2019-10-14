using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class EntryCameraNavigator : MonoBehaviour
    {
        public float MoveSpeed = 0.0f;

        private Animation anim;
        void Start()
        {
            anim = UIUtility.SafeGetComponent<Animation>(transform);
        }

        void Update()
        {
            if (anim.isPlaying)
                return;
            float mouseX = Input.GetAxis("Mouse X") * MoveSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * MoveSpeed;
            transform.localRotation = transform.localRotation * Quaternion.Euler(-mouseY, 0, 0);
            transform.localRotation = transform.localRotation * Quaternion.Euler(0, mouseX, 0);
        }

       
    }
}