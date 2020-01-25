using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class MainShipInfo
    {
        #region Shield
        public int ShieldMax;
        public int ShieldInit;

        public int currentShieldValue;

        

        #endregion

        public MainShipPowerAreaInfo powerAreaInfo;
        public MainShipControlTowerInfo controlTowerInfo;

        public MainShipInfo()
        {
            var config = Config.ConfigData.MainShipConfigData.basePropertyConfig;
            if (config != null)
            {
                ShieldMax = config.ShieldBaseMax;
                ShieldInit = config.ShieldBaseInit;
            }
        }
    }

    public class MainShipPowerAreaInfo
    {
        public enum EnergyGenerateMode
        {
            Normal,
            Overload
        }

        /// <summary>
        /// 电能产生效率
        /// </summary>
        public int PowerGenerateCount;
        public int MaxStoragePower;
        public int CurrentStoragePower;

        public byte currentOverLoadLevel = 0;

    }

    public class MainShipControlTowerInfo
    {

    }

    public class MainShipLivingAreaInfo
    {

    }

    public class MainShipHangarInfo
    {

    }

    public class MainShipWorkingAreaInfo
    {

    }
}