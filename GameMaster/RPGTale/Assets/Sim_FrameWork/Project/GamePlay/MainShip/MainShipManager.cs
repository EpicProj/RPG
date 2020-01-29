using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    /*
    * MainShip Manager
    * 
    */
namespace Sim_FrameWork
{
    public class MainShipManager :Singleton<MainShipManager>
    {
        private Dictionary<MainShipAreaType, System.Type> m_RegisterDic = new Dictionary<MainShipAreaType, System.Type>();
        private Dictionary<MainShipAreaType, MainShipAreaBase> m_AreaDic = new Dictionary<MainShipAreaType, MainShipAreaBase>();

        public MainShipInfo mainShipInfo;

        private void Awake()
        {
            
        }

        public void InitData()
        {
            mainShipInfo = new MainShipInfo();
            InitPlayerEnergyData();
        }
        #region Area Manager
        /// <summary>
        /// Area Register
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        public void Register<T>(MainShipAreaType type) where T : MainShipAreaBase
        {
            if (!m_RegisterDic.ContainsKey(type))
            {
                m_RegisterDic[type] = typeof(T);
            }
        }

        public void ClearAreaDic()
        {
            m_AreaDic.Clear();
        }

        public T FindAreaByType<T>(MainShipAreaType type) where T : MainShipAreaBase
        {
            MainShipAreaBase area = null;
            if (m_AreaDic.TryGetValue(type, out area))
            {
                return (T)area;
            }
            return null;
        }

        public MainShipAreaBase GetShipArea(MainShipAreaType type)
        {
            MainShipAreaBase area = FindAreaByType<MainShipAreaBase>(type);
            if (area == null)
            {
                System.Type tp = null;
                if (m_RegisterDic.TryGetValue(type, out tp))
                {
                    area = System.Activator.CreateInstance(tp) as MainShipAreaBase;
                }
                else
                {
                    Debug.LogError("Can not Find MainShip Area ,type=" + type);
                    return null;
                }

                if (!m_AreaDic.ContainsKey(type))
                {
                    m_AreaDic.Add(type, area);
                }
            }
            return area;
        }


        #endregion

        public bool ChangeAreaPowerLevel(short changeValue,MainShipAreaType type)
        {
            if (type == MainShipAreaType.ControlTower)
            {
                if (mainShipInfo.powerAreaInfo.ChangeEnergyLoadValue((short)-changeValue))
                {
                    mainShipInfo.controlTowerInfo.ChangePowerLevel(changeValue);
                    return true;
                }
                return false; 
            }
            else if (type == MainShipAreaType.hangar)
            {
                if (mainShipInfo.powerAreaInfo.ChangeEnergyLoadValue((short)-changeValue))
                {
                    mainShipInfo.hangarAreaInfo.ChangePowerLevel(changeValue);
                    return true;
                }
                return false;
            }
            else if (type == MainShipAreaType.LivingArea)
            {
                if (mainShipInfo.powerAreaInfo.ChangeEnergyLoadValue((short)-changeValue))
                {
                    mainShipInfo.livingAreaInfo.ChangePowerLevel(changeValue);
                    return true;
                }
                return false;
            }
            else if (type == MainShipAreaType.WorkingArea)
            {
                if (mainShipInfo.powerAreaInfo.ChangeEnergyLoadValue((short)-changeValue))
                {
                    mainShipInfo.workingAreaInfo.ChangePowerLevel(changeValue);
                    return true;
                }
                return false;
            }
            return false;
        }

        void InitPlayerEnergyData()
        {
            PlayerManager.Instance.AddEnergy(mainShipInfo.powerAreaInfo.PowerGenerateValue, ResourceAddType.month);
            PlayerManager.Instance.AddEnergy(mainShipInfo.powerAreaInfo.MaxStoragePower, ResourceAddType.max);
        }

        #region Modifier
        public void AddPowerAreaModifier(ModifierBase modifier)
        {

            //ModifierManager.Instance.AddMainShipPowerAreaModifier(mainShipInfo,)
        }
        #endregion
    }
}