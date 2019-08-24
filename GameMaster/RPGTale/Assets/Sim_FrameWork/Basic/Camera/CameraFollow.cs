using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class CameraFollow : MonoBehaviour
    {

        private enum CametaStates
        {
            Idle,
            Rotate,
            Move,
            Zoom
        }


        public Transform target;
        private CametaStates CurrentStates = CametaStates.Idle;

        private float x = 0.0f;
        private float y = 0.0f;

        private float xSpeed = 250.0f;
        private float ySpeed = 120.0f;

        private int yMinLimit = -20;
        private int yMaxLimit = 80;
        private float CameraDefaultHeight = 0.0f;
        private float MoveSpeed = 10;

        private Vector3 CameraTarget;
        private Quaternion rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));

        void Start()
        {
            CameraTarget = target.position;
            float cameraHeight = target.transform.position.z - CameraDefaultHeight;
            transform.position = rotation * new Vector3(transform.position.x, transform.position.y, cameraHeight);
            transform.LookAt(target);

            var angles = transform.eulerAngles;
            x = angles.y;
            y = angles.x;
  
        }
        public void Update()
        {
            CameraZoom();
            RotateCamera();
            CameraMove();

        }



        public void CameraZoom()
        {
            if(Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (Camera.main.fieldOfView <= 100)
                {
                    Camera.main.fieldOfView += 2;
                }
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (Camera.main.fieldOfView > 2)
                {
                    Camera.main.fieldOfView -= 2;
                }
            }
        }

        public void RotateCamera()
        {
            if (CurrentStates == CametaStates.Move)
                return;
            if (Input.GetMouseButton(2))
            {
                CurrentStates = CametaStates.Rotate;
                x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
                y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;

                y = AngleFormat(y, yMinLimit, yMaxLimit);
                var rotation = Quaternion.Euler(y, x, 0);

                transform.rotation = rotation;
            }
            CurrentStates = CametaStates.Idle;
        }

        public void CameraMove()
        {
            if (CurrentStates == CametaStates.Rotate)
                return;
            if (Input.GetKey(KeyCode.W))
            {
                CurrentStates = CametaStates.Move;
                transform.Translate(new Vector3(0.0f, 0.0f, MoveSpeed * Time.deltaTime),Space.World);
                CameraTarget = transform.position;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                CurrentStates = CametaStates.Move;
                transform.Translate(new Vector3(0.0f, 0.0f, -MoveSpeed * Time.deltaTime), Space.World);
                CameraTarget = transform.position;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                CurrentStates = CametaStates.Move;
                transform.Translate(new Vector3(-MoveSpeed * Time.deltaTime,0.0f, 0.0f), Space.World);
                CameraTarget = transform.position;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                CurrentStates = CametaStates.Move;
                transform.Translate(new Vector3(MoveSpeed * Time.deltaTime, 0.0f, 0.0f), Space.World);
                CameraTarget = transform.position;
            }
            CurrentStates = CametaStates.Idle;

        }

        private static float AngleFormat(float angle,float min,float max)
        {
            if (angle < -360)
                angle += 360;
            if (angle > 360)
                angle -= 360;
            return Mathf.Clamp(angle, min, max);
        }

    }

    public class CameraParam
    {

    }
}