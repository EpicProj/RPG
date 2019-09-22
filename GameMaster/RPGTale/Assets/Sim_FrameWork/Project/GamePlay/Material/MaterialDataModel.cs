using UnityEngine;

namespace Sim_FrameWork {
    public struct MaterialDataModel : BaseDataModel
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
            _id = -1;
            _name = null;
            _desc = null;
            _icon = null;
        }

        private int _id;
        public int ID
        {
            get {return _id;}
            set {_id = value;}
        }

        private string _name;
        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                    _name = MaterialModule.GetMaterialName(_id);
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
                    _desc = MaterialModule.GetMaterialDesc(_id);
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
                    _icon = MaterialModule.GetMaterialSprite(_id);
                return _icon;
            }
            set { }
        }
    
    }

}