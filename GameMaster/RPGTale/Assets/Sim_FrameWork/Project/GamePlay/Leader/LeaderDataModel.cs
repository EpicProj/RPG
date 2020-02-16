namespace Sim_FrameWork
{
    public struct LeaderDataModel : BaseDataModel
    {
        private int _leaderID;
        public int LeaderID { get { return _leaderID; } set { _leaderID = value; } }

        public bool CreateLeaderModel(int leaderiD)
        {
            if (LeaderModule.GetLeaderPresetDataByKey(leaderiD) != null)
            {
                _leaderID = leaderiD;
                return true;
            }
            return false;
        }


        private LeaderInfo _info;
        public LeaderInfo Info
        {
            get
            {
                if (_info == null)
                    _info = LeaderInfo.CreateLeaderInfo_Preset(_leaderID);
                return _info;
            }
        }

    }
}