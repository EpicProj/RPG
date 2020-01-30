using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class MainShipAreaManagerBase : MonoSingleton<MainShipAreaManagerBase>
    {
        public ushort MapSizeX;
        public ushort MapSizeY;

        public GameObject mapFloorModel;

        protected override void Awake()
        {
            base.Awake();
        }

        public void LoadMapFloorModel(string modelPath)
        {
            ObjectManager.Instance.InstantiateObjectAsync(modelPath, OnMapFloorLoadFinish, LoadResPriority.RES_HIGHT);
        }
        
        private void OnMapFloorLoadFinish(string path,Object obj,object param1, object param2, object param3)
        {
            mapFloorModel = obj as GameObject;
            var modelRoot = transform.FindTransfrom("AreaFloor");
            mapFloorModel.transform.SetParent(modelRoot, false);
        }


    }

}