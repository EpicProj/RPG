using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class ManufactoryBase : FactoryBase
    {
        public float Manufacturingspeed = 0f;


        public Dictionary<int, ushort> InputMaterialDic = new Dictionary<int, ushort>();
        public Dictionary<int, ushort> OutputMaterialDic = new Dictionary<int, ushort>();
        public Dictionary<int, ushort> ByProductMaterialDic = new Dictionary<int, ushort>();


        public override void InitData()
        {
            Manufacturingspeed = FactoryModule.Instance.GetManufactureSpeed(factoryID);
            //InputMaterialDic = FactoryModule.Instance.GetManufactureMaterialList(factoryID, FactoryModule.FactoryManuMaterialType.Input);
            //OutputMaterialDic = FactoryModule.Instance.GetManufactureMaterialList(factoryID, FactoryModule.FactoryManuMaterialType.Output);
            //ByProductMaterialDic = FactoryModule.Instance.GetManufactureMaterialList(factoryID, FactoryModule.FactoryManuMaterialType.Byproduct);
        }

    }
}