using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork.Config
{
    public class MainShipConfig
    {
        public MainShipBasePropertyConfig basePropertyConfig;
        public PowerAreaConfig powerAreaConfig;

        public MainShipConfig LoadMainShipConfig()
        {
            JsonReader reader = new JsonReader();
            var data = reader.LoadJsonDataConfig<MainShipConfig>(JsonConfigPath.MainShipConfigJsonPath);
            basePropertyConfig = data.basePropertyConfig;
            powerAreaConfig = data.powerAreaConfig;
            return data;
        }

    }

    public class MainShipBasePropertyConfig
    {
        /// <summary>
        /// 护盾基础最大值
        /// </summary>
        public int ShieldBaseMax;
        /// <summary>
        /// 护盾默认初始值
        /// </summary>
        public int ShieldBaseInit;
        /// <summary>
        /// 每回合回盾，未受攻击
        /// </summary>
        public int ShieldChargeEachRound_NotAttack;
        /// <summary>
        /// 每回合回盾，受到攻击
        /// </summary>
        public int ShieldCahrgeEachRound_UnderAttack;
        /// <summary>
        /// 初始减伤
        /// </summary>
        public float DamageReduceInit;

        public float SpeedBase;

        public List<ShieldLevelMap> shieldLevelMap;

        public class ShieldLevelMap
        {
            public int Level;
            public float DamageReduceRate;
            public int ShieldChargeEachRound_NotAttack;
            public int ShieldCahrgeEachRound_UnderAttack;
        }

    }

    public class PowerAreaConfig
    {
        
        public List<OverLoadLevelMap> overLoadMap;

        public class OverLoadLevelMap
        {
            public int Level;
            public float FuelConsumeRate;
            public float PowerPruduceRate;
            public float CloseDownRate;
        }
    }

}