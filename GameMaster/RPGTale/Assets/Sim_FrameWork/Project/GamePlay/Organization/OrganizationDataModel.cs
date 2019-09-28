using UnityEngine;

namespace Sim_FrameWork
{
    public struct OrganizationDataModel : BaseDataModel
    {
        public bool Create(int id)
        {
            if (OrganizationModule.GetOrganizationDataByID(id) == null)
                return false;
            ID = id;
            return true;
        }
        public void CleanUp()
        {
            _id = -1;
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
                    _name = OrganizationModule.GetOrganizationName(_id);
                return _name;
            }
            set { }
        }
        private string _name_En;
        public string Name_En
        {
            get
            {
                if (string.IsNullOrEmpty(_name_En))
                    _name_En = OrganizationModule.GetOrganizationName_En(_id);
                return _name_En;
            }
            set { }
        }
        private string _briefDesc;
        public string BriefDesc
        {
            get
            {
                if (string.IsNullOrEmpty(_briefDesc))
                    _briefDesc = OrganizationModule.GetOrganizationBriefDesc(_id);
                return _briefDesc;
            }
            set { }
        }
        private Sprite _icon;
        public Sprite Icon
        {
            get
            {
                if (_icon == null)
                    _icon = OrganizationModule.GetOrganizationSprite(_id);
                return _icon;
            }
            set { }
        }
        private Sprite _iconBig;
        public Sprite IconBig
        {
            get
            {
                if (_iconBig == null)
                    _iconBig = OrganizationModule.GetOrganizationSpriteBig(_id);
                return _iconBig;
            }
            set { }
        }
        private OrganizationTypeModel _typeModel;
        public OrganizationTypeModel TypeModel
        {
            get
            {
                if(_typeModel.ID==0)
                {
                    _typeModel = new OrganizationTypeModel();
                    _typeModel.Create(_id);
                }
                return _typeModel;
            }
            set { }
        }


    }

    public struct OrganizationTypeModel : BaseDataModel
    {
        public bool Create(int id)
        {
            if (OrganizationModule.FetchOrganizationType(id) == null)
                return false;
            ID = id;
            return true;
        }
        public void CleanUp()
        {
            _id = -1;
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
                    _name = OrganizationModule.GetTypeName(_id);
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
                    _desc = OrganizationModule.GetTypeDesc(_id);
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
                    _icon = OrganizationModule.GetTypeIcon(_id);
                return _icon;
            }
            set { }
        }
    }
}