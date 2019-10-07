using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;

        public Vector3 targetOffset;
        public float distance = 5.0f;
        public float maxDistance = 20;
        public float minDistance = 0.6f;

        [Header("Speed")]
        public float xSpeed = 200.0f;
        public float ySpeed = 200.0f;
        private int yMinLimit = -80;
        private int yMaxLimit = 80;
        /// <summary>
        /// Zoom
        /// </summary>
        public int zoomRate = 30;
        public float panSpeed = 0.3f;
        public float zoomDamp = 5.0f;

        private float currentDistance;
        private float targetDistance;
        private Quaternion currentRotation;
        private Quaternion targetRotation;
        private Quaternion rotation;
        private Vector3 position;

        private float xDeg = 0.0f;
        private float yDeg = 0.0f;

        void Start()
        {
            InitData();
        }
        void InitData()
        {
            if (!target)
            {

            }
            distance = Vector3.Distance(transform.position, target.position);
            currentDistance = distance;
            targetDistance = distance;

            position = transform.position;
            rotation = transform.rotation;
            currentRotation = transform.rotation;
            targetRotation = transform.rotation;

            xDeg = Vector3.Angle(Vector3.right, transform.right);
            yDeg = Vector3.Angle(Vector3.up, transform.up);

        }

        void LateUpdate()
        {
            //if (Input.GetMouseButton(1))
            //{
            //    targetDistance -= -Input.GetAxis("Mouse X") * Time.deltaTime * zoomRate * 0.125f * Mathf.Abs(targetDistance);
            //    targetDistance-= -Input.GetAxis("Mouse Y") * Time.deltaTime * zoomRate * 0.125f * Mathf.Abs(targetDistance);
            //}
            if(Input.GetMouseButton(1))
            {
                target.rotation = transform.rotation;
                target.Translate(Vector3.right * -Input.GetAxis("Mouse X") * panSpeed);
                target.Translate(transform.up * -Input.GetAxis("Mouse Y") * panSpeed, Space.World);
            }
            //Middle mouse
            else if (Input.GetMouseButton(2))
            {
                xDeg += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                yDeg -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
                ///Format angle
                yDeg = AngleFormat(yDeg, yMinLimit, yMaxLimit);

                targetRotation = Quaternion.Euler(yDeg, xDeg, 0);
                currentRotation = transform.rotation;

                rotation = Quaternion.Lerp(currentRotation, targetRotation, Time.deltaTime * zoomDamp);
                transform.rotation = rotation;
            }

            targetDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs(targetDistance);
            targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);
            //Smooth
            currentDistance = Mathf.Lerp(currentDistance, targetDistance, Time.deltaTime * zoomDamp);
            position = target.position - (rotation * Vector3.forward * currentDistance + targetOffset);
            transform.position = position;


        }



  
        private float AngleFormat(float angle,float min,float max)
        {
            if (angle < -360)
                angle += 360;
            if (angle > 360)
                angle -= 360;
            return Mathf.Clamp(angle, min, max);
        }

    }

}