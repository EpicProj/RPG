using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public struct SaveDataModel : BaseDataModel
    {
        public bool Create(int id)
        {
            ID = id;
            return true;
        }

        private int _id;
        public int ID { get { return _id; } set { _id = value; } }

        private string _name;
        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                    _name = GameDataSaveManager.Instance.GetSaveDataBySaveID(_id).SaveName;
                return _name;
            }
            set { }
        }

        private string _organization;
        public string Organization
        {
            get
            {
                if (string.IsNullOrEmpty(_organization))
                    _organization = GameDataSaveManager.Instance.GetSaveDataBySaveID(_id).OrganizatioName;
                return _organization;
            }
            set { }
        }

        private string _date;
        public string Date
        {
            get
            {
                if (string.IsNullOrEmpty(_date))
                    _date = GameDataSaveManager.Instance.GetSaveDataBySaveID(_id).SaveDate;
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
                    _gameTime = GameDataSaveManager.Instance.GetSaveDataBySaveID(_id).GameTime.ToString()+"h";
                return _gameTime;
            }
            set { }
        }

        public void CleanUp()
        {
            _id = -1;
            _name = null;
            _organization = null;
            _date = null;
        }

    
    }
}