using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class SolarSystemManager : MonoSingleton<SolarSystemManager>
    {

        private Transform CameraNevigator;

        private float MoveTime = 1f;
        private float RotationSpeed = 2.5f;
        private float timer = 0f;

        Vector3 targetCameraPos;
        Quaternion targetCameraRotation;

        float distance;


        public bool cameraMove = false;

        protected override void Awake()
        {
            base.Awake();
            CameraNevigator = GameObject.Find("CameraNevigator").transform;
        }

        private void Update()
        {
            if (cameraMove)
            {
                StartExploreMissionCamera();
            }
        }

        public void MoveToExploreMissionPoint(int exploreID)
        {
            targetCameraPos = ExploreModule.GetExploreMissionCameraPos(exploreID);
            targetCameraRotation = ExploreModule.GetExploreMissionCameraRotation(exploreID);
            distance = (CameraNevigator.position - targetCameraPos).magnitude;
            cameraMove = true;
        }


        private void StartExploreMissionCamera()
        {
            CameraNevigator.position = Vector3.MoveTowards(CameraNevigator.position, targetCameraPos, (distance/MoveTime)*Time.deltaTime);
            CameraNevigator.rotation = Quaternion.Slerp(CameraNevigator.rotation, targetCameraRotation, RotationSpeed * Time.deltaTime );
            timer += Time.deltaTime;
            if (timer >= MoveTime)
            {
                timer = 0;
                cameraMove = false;
            }
        }


    }
}