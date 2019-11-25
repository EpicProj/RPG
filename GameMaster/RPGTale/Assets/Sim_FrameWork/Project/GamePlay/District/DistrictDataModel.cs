using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public struct DistrictDataModel : BaseDataModel
    {

        public bool Create(int id)
        {
            if (MaterialModule.GetMaterialByMaterialID(id) == null)
                return false;
            _id = id;
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

        private string _name;
        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                    _name = DistrictModule.GetDistrictName(_id);
                return _name;
            }
            set { }
        }

        private string _desc;
        public string Desc
        {
            get
            {
                if (string.IsNullOrEmpty(_desc))
                    _desc = DistrictModule.GetDistrictDesc(_id);
                return _name;
            }
        }

   
    }
}