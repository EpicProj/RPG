using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sim_FrameWork
{
    public class TimeModule : Singleton<TimeModule>
    {
        public enum Season
        {
            Spring =1,
            Summer =2,
            Autumn =3,
            Winter =4 
        }

        public enum Month
        {
            January=1,
            February=2,
            March=3,
            April=4,
            May=5,
            June=6,
            July=7,
            August=8,
            September=9,
            October=10,
            November=11,
            December=12
        }

        public int CurrentYear;
        public int CurrentMonth;

        public int ConvertMonthToSeason(int month)
        {
            double f = Convert.ToDouble(month) / 3f;
            if (f > Convert.ToInt32(f))
            {
                return Convert.ToInt32(f) + 1;
            }
            return Convert.ToInt32(f);
        }
        public Season IntConvertToSeason(int i)
        {
            if (Enum.IsDefined(typeof(Season), i))
            {
                return (Season)Enum.ToObject(typeof(Season), i);
            }
            Debug.LogError("SeasonConvertError Season=" + i);
            return Season.Spring;
        }
        public Season GetCurrentSeason()
        {
            return IntConvertToSeason(ConvertMonthToSeason(CurrentMonth));
        }


    }

    public class TimeDataConfig
    {
        public int OriginalYear;
        public int OriginalMonth;
        public float RealSecondsPerMonth;
    }
}