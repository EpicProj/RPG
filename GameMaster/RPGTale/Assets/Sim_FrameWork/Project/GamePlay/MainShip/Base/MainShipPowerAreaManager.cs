using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork {
    public class MainShipPowerAreaManager : MainShipAreaManagerBase
    {
        private const string PowerArea_Map_Floor_Path = "Assets/Objects/MainShipFloor/Area/PowerArea/PowerArea_BaseFloor.prefab";

        protected override void Awake()
        {
            base.Awake();
            InitMapData();
        }

        private void InitMapData()
        {
            var config = Config.ConfigData.MainShipMapConfig.powerAreaConfig;
            if (config != null)
            {
                MapSizeX = config.mapLength;
                MapSizeY = config.mapWidth;
                GridManager.Instance.InitMapGrid(MapSizeX, MapSizeY);
            }
        }

        public void Start()
        {

        }

        public void LoadPowerArea()
        {
            LoadMapFloorModel(PowerArea_Map_Floor_Path);
        }
    }
}