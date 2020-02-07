using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public struct SaveDataGroupModel : BaseDataModel
    {
        public bool Create(int id)
        {
            ID = id;
            return true;
        }

        private int _id;
        public int ID { get { return _id; } set { _id = value; } }

        private GameSaveDataItem _latestSaveData;
        public GameSaveDataItem LastestSaveData
        {
            get
            {
                if (_latestSaveData == null)
                    _latestSaveData = GameDataSaveManager.Instance.GetLatestSaveData(_id);
                return _latestSaveData;
            }
        }


        private string _name;
        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                    _name = LastestSaveData.saveName;
                return _name;
            }
            set { }
        }

        private string _date;
        public string Date
        {
            get
            {
                if (string.IsNullOrEmpty(_date))
                    _date = LastestSaveData.SaveDate;
                return _date;
            }
            set { }
        }

        private string _gameTime;
        public string GameTime
        {
            get
            {
                if (string.IsNullOrEmpty(_gameTime))
                    _gameTime = LastestSaveData.GameTime.ToString()+"h";
                return _gameTime;
            }
            set { }
        }

    }

    public struct SaveDataItemModel : BaseDataModel
    {
        public bool Create(int groupID, int indexID)
        {
            if (GameDataSaveManager.Instance.GetSaveNavigatorData(groupID, indexID) == null)
                return false;
            _groupID = groupID;
            _indexID = indexID;
            return true;
        }

        public int _groupID;
        public int _indexID;

        private GameSaveDataItem _SaveData;
        public GameSaveDataItem SaveData
        {
            get
            {
                if (_SaveData == null)
                    _SaveData = GameDataSaveManager.Instance.GetSaveNavigatorData(_groupID,_indexID);
                return _SaveData;
            }
        }


        private string _name;
        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                    _name = SaveData.saveName;
                return _name;
            }
            set { }
        }

        private string _date;
        public string Date
        {
            get
            {
                if (string.IsNullOrEmpty(_date))
                    _date = SaveData.SaveDate;
                return _date;
            }
            set { }
        }

        private string _gameTime;
        public string GameTime
        {
            get
            {
                if (string.IsNullOrEmpty(_gameTime))
                    _gameTime = SaveData.GameTime.ToString() + "h";
                return _gameTime;
            }
            set { }
        }

    }
}