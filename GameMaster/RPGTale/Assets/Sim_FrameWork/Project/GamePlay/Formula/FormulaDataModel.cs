using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public struct FormulaDataModel : BaseDataModel
    {
        public bool Create(int id)
        {
            if (FormulaModule.GetFormulaDataByID(id) == null)
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
                    _name= FormulaModule.GetFormulaName(_id);
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
                    _desc = FormulaModule.GetFormulaDesc(_id);
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
                    _icon = FormulaModule.GetFormulaIcon(_id);
                return _icon;
            }
            set { }
        }

       
    }
}