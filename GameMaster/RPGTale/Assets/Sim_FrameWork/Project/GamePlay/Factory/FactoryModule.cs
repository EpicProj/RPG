using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork {
    public class FactoryModule : MonoSingleton<FactoryModule> {

        public enum FactoryType
        {
            Manufacture,
            Raw,
            Science,
            Energy
        }

        public List<Factory> factoryList;
        public List<Factory_Raw> factory_RowList;
        public List<FactoryTypeData> factoryTypeDataList;

        private bool HasInit = false;

        private void InitData()
        {
            factoryList = FactoryMetaDataReader.GetFactoryData();
            factory_RowList = FactoryMetaDataReader.GetFactoryRowData();
            factoryTypeDataList = FactoryMetaDataReader.GetFactoryTypeData();

            CheckTypeValid();
            HasInit = true;
        }



        private bool CheckTypeValid()
        {
            List<string> types = new List<string>();
            foreach(var type in factoryTypeDataList)
            {
                string typestr = type.Type;
                if(Enum.IsDefined(typeof(FactoryType), typestr) == false)
                {
                    Debug.LogError("FacotyType InValid! Type=" + typestr);
                    return false;
                }
            }
            return true;
        }

    }
}