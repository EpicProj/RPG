using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sim_FrameWork
{
    public struct FunctionBlockDataModel : BaseDataModel
    {
        public bool Create(int id)
        {
            if (FunctionBlockModule.GetFunctionBlockByBlockID(id) == null)
                return false;
            ID = id;
            return true;
        }

        public void CleanUp()
        {
            _id = -1;
            _guid = null;
            _name = null;
        }


        private int _id;
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _guid;
        public string GUID
        {
            get
            {
                if(_guid == null)
                    _guid = Guid.NewGuid().ToString();
                return _guid;
            }
            set { }
        }

        private string _name;
        public string Name
        {
            get
            {
                if (_name == null)
                    _name = FunctionBlockModule.GetFunctionBlockName(_id);
                return _name;
            }
            set { }
        }

        private string _desc;
        public string Desc
        {
            get
            {
                if (_desc == null)
                    _desc = FunctionBlockModule.GetFunctionBlockDesc(_id);
                return _desc;
            }
            set { }
        }

 
   
    }
}