using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class FunctionBlock_Smelt : ManufactoryBase
    {
        public List<BlockLevelData> leveldata;
        private string InputDesc;
        private string InputIconPath;
        private string OutputDesc;
        private string OutputIconPath;
        private string ByproductDesc;
        private string ByproductIconPath;

        //Factory

        private BoxCollider factoryCollider;
        RaycastHit hit;

        public override void Awake()
        {
            FunctionBlockModule.Instance.InitData();
            MaterialModule.Instance.InitData();
            functionBlockID = 100;
            base.Awake();
            factoryCollider = gameObject.GetComponent<BoxCollider>();

            //GameObject obj = MaterialModule.Instance.InitMaterialObj(100);
            //obj.transform.SetParent(GameObject.Find("Canvas").transform,false);
        }

        public override void Update()
        {
            CheckMouseButtonDown();
        }
        public override void InitData()
        {
            base.InitData();
            Config.ManufactoryConfigReader reader= new Config.ManufactoryConfigReader();
            FunctionBlock_Smelt_Config config = new FunctionBlock_Smelt_Config();
            leveldata = config.leveldata;
            InputDesc = config.InputDesc;
            InputIconPath = config.InputIconPath;
            OutputDesc = config.OutputDesc;
            OutputIconPath = config.OutputIconPath;
            ByproductDesc = config.ByproductDesc;
            ByproductIconPath = config.ByproductIconPath;

        }

        public void Product()
        {

        }


        public override void OnPlaceFunctionBlock()
        {
            base.OnPlaceFunctionBlock();
        }

        private void CheckMouseButtonDown()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(ray,out hit))
                {
                    Debug.Log(hit.collider.gameObject.name);
                }
            }
        }
     
    }

    public class FunctionBlock_Smelt_Config
    {
        public List<BlockLevelData> leveldata;
        public string InputDesc;
        public string InputIconPath;
        public string OutputDesc;
        public string OutputIconPath;
        public string ByproductDesc;
        public string ByproductIconPath;
    }
}