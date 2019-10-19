using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public struct MaterialStorageModel : BaseDataModel
    {
        public bool Create(int id)
        {
            if (MaterialModule.GetMaterialByMaterialID(id) == null)
                return false;
            ID = id;
            return true;
        }

        public void CleanUp()
        {

        }

        private int _id;
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private MaterialDataModel _maModel;
        public MaterialDataModel MaModel
        {
            get
            {
                if (_maModel.ID == 0)
                {
                    _maModel = new MaterialDataModel();
                    _maModel.Create(_id);
                }
                return _maModel;
            }
            set { }
        }
    

      
    }
}