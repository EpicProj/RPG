using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public struct BuildPanelModel : BaseDataModel
    {

        public bool Create(int id)
        {
            if (PlayerModule.GetBuildingPanelDataByKey(id) != null)
            {
                _id = id;
                return true;
            }
            return false;     
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

        private BuildingPanelData _buildData;
        public BuildingPanelData BuildData
        {
            get
            {
                if (_buildData == null)
                    _buildData= PlayerModule.GetBuildingPanelDataByKey(_id);
                return _buildData;
            }
            set { }
        }
   
    }
}