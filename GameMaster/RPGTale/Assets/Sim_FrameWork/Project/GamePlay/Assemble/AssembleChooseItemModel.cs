using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public struct AssembleChooseItemModel : BaseDataModel
    {
        public bool Create(int id)
        {
            if (PlayerManager.Instance.GetAssemblePartDesignInfo((ushort)id) == null)
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
                    _info = PlayerManager.Instance.GetAssemblePartDesignInfo((ushort)_id);
                return _info;
            }
            set { }
        }
    }

    public struct AssembleTypePresetModel : BaseDataModel
    {
        public bool Create(int id)
        {
            if (AssembleModule.GetAssemblePartDataByKey(id)==null)
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

        private AssembleParts _partsMeta;
        public AssembleParts PartsMeta
        {
            get
            {
                if (_partsMeta == null)
                    _partsMeta = AssembleModule.GetAssemblePartDataByKey(_id);
                return _partsMeta;
            }
        }

        private AssemblePartTypePresetData _presetInfo;
        public AssemblePartTypePresetData PresetInfo
        {
            get
            {
                if (_presetInfo == null)
                    _presetInfo = new AssemblePartTypePresetData(PartsMeta.ModelTypeID);
                return _presetInfo;
            }
            set { }
        }
    }

    public struct AssembleShipTypePresetModel : BaseDataModel
    {
        public bool Create(int id)
        {
            if (AssembleModule.GetWarshipDataByKey(id) == null)
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

        private AssembleShipTypePresetData _presetInfo;
        public AssembleShipTypePresetData PresetInfo
        {
            get
            {
                if (_presetInfo == null)
                    _presetInfo = new AssembleShipTypePresetData(ID);
                return _presetInfo;
            }
            set { }
        }
    }


    public struct AssembleShipModel : BaseDataModel
    {
        public bool Create(int id)
        {
            if (!PlayerManager.Instance.AssembleShipDesignDataDic.ContainsKey((ushort)id))
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

        private AssembleShipInfo _info;
        public AssembleShipInfo Info
        {
            get
            {
                if (_info == null)
                    _info = PlayerManager.Instance.GetAssembleShipInfo((ushort)ID);
                return _info;
            }
            set { }
        }
    }
}