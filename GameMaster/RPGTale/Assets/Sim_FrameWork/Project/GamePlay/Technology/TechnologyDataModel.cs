using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public struct TechnologyDataModel : BaseDataModel
    {
        public bool Create(int id)
        {
            if (TechnologyModule.GetTechDataByID(id) != null)
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

        private string _name;
        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                    _name = TechnologyModule.GetTechName(_id);
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
                    _desc = TechnologyModule.GetTechDesc(_id);
                return _desc;
            }
            set { }
        }

        private Sprite _icon;
        public Sprite Icon
        {
            get
            {
                if (_icon == null)
                    _icon = TechnologyModule.GetTechIcon(_id);
                return _icon;
            }
            set { }
        }

        private ushort _techCost;
        public ushort TechCost
        {
            get
            {
                if (_techCost == 0)
                    _techCost = TechnologyModule.GetTechDataByID(_id).Cost;
                return _techCost;
            }
            set { }
        }
   

      
    }
}