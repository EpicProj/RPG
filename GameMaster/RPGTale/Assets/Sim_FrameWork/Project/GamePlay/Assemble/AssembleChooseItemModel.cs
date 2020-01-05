﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public struct AssembleChooseItemModel : BaseDataModel
    {
        public bool Create(int id)
        {
            if (PlayerManager.Instance.GetAssemblePartInfo((ushort)id) == null)
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

        private AssemblePartInfo _info;
        public AssemblePartInfo Info
        {
            get
            {
                if (_info == null)
                    _info = PlayerManager.Instance.GetAssemblePartInfo((ushort)_id);
                return _info;
            }
            set { }
        }

    

      
    }
}