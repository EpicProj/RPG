using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class SolarSystemManager : MonoSingleton<SolarSystemManager>
    {

        protected override void Awake()
        {
            base.Awake();
            CameraNevigator = GameObject.Find("CameraNevigator").transform;
            InitPlanetReference();
        }

        private void Update()
        {
            if (cameraMove)
            {
                StartExploreMissionCamera();
            }
        }

        #region Camera
        private Transform CameraNevigator;

        private float MoveTime = 1f;
        private float RotationSpeed = 2.5f;
        private float timer = 0f;

        Vector3 targetCameraPos;
        Quaternion targetCameraRotation;

        float distance;

        bool cameraMove = false;

        /// <summary>
        /// 行星移动
        /// </summary>
        /// <param name="exploreID"></param>
        public void MoveToExploreMissionPoint(int exploreID)
        {
            targetCameraPos = ExploreModule.GetExploreMissionCameraPos(exploreID);
            targetCameraRotation = ExploreModule.GetExploreMissionCameraRotation(exploreID);
            distance = (CameraNevigator.position - targetCameraPos).magnitude;
            cameraMove = true;
            timer = 0;
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
        #endregion

        #region Planet

        private Transform Planet_Earth;
        private Transform[] _planetEarth_Point;
        private Transform Planet_Mars;
        private Transform Planet_Sun;
        private Transform Planet_Jupiter;
        private Dictionary<int,Vector3> _PlanetJupiter_Point =new Dictionary<int, Vector3> ();

        void InitPlanetReference()
        {
            Planet_Earth = UIUtility.FindTransfrom(transform, "Planet_Earth");
            Planet_Mars = UIUtility.FindTransfrom(transform, "Planet_Mars");
            Planet_Sun = UIUtility.FindTransfrom(transform, "Planet_Sun");
            Planet_Jupiter = UIUtility.FindTransfrom(transform, "Planet_Jupiter");

            var JupiterPointTrans = UIUtility.FindTransfrom(Planet_Jupiter, "PointContent");
            for(int i = 0; i < JupiterPointTrans.childCount; i++)
            {
                _PlanetJupiter_Point.Add(i, JupiterPointTrans.GetChild(i).position);
            }
        }


        /// <summary>
        /// 获取探险点位坐标 / WorldPosition
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Vector3 GetPointPosition(ExplorePointData data)
        {
            Vector3 result = new Vector3();
            if (data == null)
                return result;
            switch (ExploreModule.GetPointMapData(data.PointAreaNevigator))
            {
                case "Planet_Jupiter":
                    _PlanetJupiter_Point.TryGetValue(data.PointPlanetpointNevigator, out result);
                    return result;
            }
            return result;
        }

        public Vector2 GetPointPositionUI(ExplorePointData data)
        {
            var worldPos = GetPointPosition(data);
            var pos= Camera.main.WorldToScreenPoint(worldPos);
            return InventoryManager.Instance.ScreenPointToLocalPoint(pos);
        }

        #endregion

    }
}