using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public struct OrderDataModel : BaseDataModel
    {
        public bool Create(int id)
        {
            if (OrderModule.GetOrderDataByID(id) == null)
                return false;
            ID = id;
            return true;
        }

        public void CleanUp()
        {
            _id = -1;
            _name = null;
            _desc = null;
            _icon = null;
        }

        private int _id;
        public int ID { get { return _id; } set { _id = value; } }

        private string _name;
        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                    _name = OrderModule.GetOrderName(_id);
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
                    _desc = OrderModule.GetOrderDesc(_id);
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
                    _icon = OrderModule.GetOrderBGPath(_id);
                return _icon;
            }
            set { }
        }

        private Dictionary<MaterialDataModel, int> _orderContentDic;
        public Dictionary<MaterialDataModel, int> OrderContentDic
        {
            get
            {
                if (_orderContentDic == null)
                    _orderContentDic = OrderModule.GetOrderContent(_id);
                return _orderContentDic;
            }
            set { }
        }

    }

}