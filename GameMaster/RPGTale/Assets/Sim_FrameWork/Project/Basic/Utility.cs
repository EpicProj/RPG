using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;

namespace Sim_FrameWork
{
    public class Utility 
    {
        #region Math

        public static int TryParseInt(string str)
        {
            int result;
            if (int.TryParse(str, out result) == false)
            {
                Debug.LogError("Parse Int Error,str=" + str);
            }
            return result;
        }

        public static float TryParseFloat(string str)
        {
            float result;
            if (float.TryParse(str, out result) == false)
            {
                Debug.LogError("Parse Float Error,Str=" + str);
            }
            return result;
        }


        public static List<int> TryParseIntList(string str, char split)
        {
            List<int> result = new List<int>();
            if (string.IsNullOrEmpty(str))
            {
                Debug.LogWarning("Parse Int List Error");
                return result;
            }

            string[] s = str.Split(split);
            for (int i = 0; i < s.Length; i++)
            {
                int n;
                if (int.TryParse(s[i], out n))
                {
                    result.Add(n);
                }
                else
                {
                    Debug.LogError("parse int error, string=" + s[i]);
                    continue;
                }
            }
            return result;
        }

        public static List<float> TryParseFloatList(string str, char split)
        {
            List<float> result = new List<float>();
            if (string.IsNullOrEmpty(str))
            {
                Debug.LogWarning("Parse Int List Error");
                return result;
            }

            string[] s = str.Split(split);
            for (int i = 0; i < s.Length; i++)
            {
                float n;
                if (float.TryParse(s[i], out n))
                {
                    result.Add(n);
                }
                else
                {
                    Debug.LogError("parse int error, string=" + s[i]);
                    continue;
                }
            }
            return result;
        }


        public static bool CheckValueInRange(float min, float max, float value, string debug = null)
        {
            if (value < min || value > max)
            {
                Debug.LogError("Value not in range,value=" + value + "  , DebugContent=" + debug);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Random
        /// </summary>
        public static List<T> GetRandomList<T>(List<T> list, int count) where T : RandomObject
        {
            if (list == null || list.Count <= count || count <= 0)
                return list;

            int totalWeights = 0;
            for(int i = 0; i < list.Count; i++)
            {
                totalWeights += list[i].Weight + 1;
            }

            System.Random ran = new System.Random(GetRandomSeed());
            List<KeyValuePair<int, int>> wlist = new List<KeyValuePair<int, int>>();

            for(int i = 0; i < list.Count; i++)
            {
                int w = (list[i].Weight + 1) + ran.Next(0, totalWeights);
                wlist.Add(new KeyValuePair<int, int>(i, w));
            }

            wlist.Sort(delegate (KeyValuePair<int, int> kvp1, KeyValuePair<int, int> kvp2)
            {
                return kvp2.Value - kvp1.Value;
            });

            List<T> newList = new List<T>();
            for(int i = 0; i < count; i++)
            {
                T en = list[wlist[i].Key];
                newList.Add(en);
            }
            return newList;
        }

        private static int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        /// <summary>
        /// 时间转化
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static string TimeFormat(int seconds)
        {
            TimeSpan ts = new TimeSpan(0, 0, 0, Convert.ToInt32(seconds));
            string str = "";
            if (ts.Hours > 0)
            {
                str = ts.Hours.ToString().PadLeft(2, '0') + ":" + ts.Minutes.ToString().PadLeft(2, '0') + ":" + ts.Seconds.ToString().PadLeft(2, '0') ;
            }
            if(ts.Hours==0 && ts.Minutes > 0)
            {
                str = ts.Minutes.ToString().PadLeft(2, '0')+ ":" + ts.Seconds.ToString().PadLeft(2, '0'); 
            }
            if(ts.Hours==0 && ts.Minutes == 0)
            {
                str = "00:" + ts.Seconds.ToString().PadLeft(2,'0');
            }
            return str;
        }
        
        /// <summary>
        /// Parse Compare
        /// </summary>
        /// <param name="compareValue"></param>
        /// <param name="parseStr"></param>
        /// <returns></returns>
        public static bool ParseGeneralCompare(int compareValue , string parseStr)
        {
            var strList = TryParseStringList(parseStr, ',');
            if (strList.Count != 2 || strList==null)
            {
                Debug.LogError("Parse CompareData Error ,str= " + parseStr);
                return true;
            }
            else
            {
                var value = TryParseInt(strList[1]);
                if (string.Compare(strList[0], ">=") == 0)
                {
                    return compareValue >= value;
                }else if (string.Compare(strList[0], ">") == 0)
                {
                    return compareValue > value;
                }
                else if (string.Compare(strList[0], "=") == 0)
                {
                    return compareValue == value;
                }
                else if (string.Compare(strList[0], "<=") == 0)
                {
                    return compareValue <= value;
                }
                else if (string.Compare(strList[0], "<") == 0)
                {
                    return compareValue < value;
                }
                else
                {
                    Debug.LogError("Parse CompareData Error ,str= " + parseStr);
                    return true;
                }
            }
        }



        #endregion


        public enum SpriteType
        {
            png,
            jpg
        }


        public static Sprite LoadSprite(string SpritePath,SpriteType type)
        {
            string TargetPath = "Assets/" + SpritePath +"."+type.ToString();
            Sprite sprite = null;
            try
            {
                sprite = ResourceManager.Instance.LoadResource<Sprite>(TargetPath);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            return sprite;
        }


        public static List<string> TryParseStringList(string str, char split)
        {
            List<string> result = new List<string> ();
            if (string.IsNullOrEmpty(str))
            {
                Debug.LogWarning("Parse string List Error");
                return result;
            }

            string[] s = str.Split(split);
            for (int i = 0; i < s.Length; i++)
            {
                result.Add(s[i]);
            }
            return result;
        }

        public static Vector2 TryParseIntVector2(string str,char split)
        {
            Vector2 result = new Vector2 ();
            string[] s = str.Split(split);
            if (string.IsNullOrEmpty(str) || s.Length != 2)
            {
                Debug.LogWarning("Parse IntVector2 Error");
                return result;
            }
            int x, y;     
            if(int.TryParse(s[0],out x) && int.TryParse(s[1],out y))
            {
                result = new Vector2(x, y);
            }
            else
            {
                Debug.LogWarning("Parse IntVector2 Error, s="+s);
            }
            return result;
        }

        public static string ParseStringParams(string content,string[] replaceValue)
        {
            int index = 1;
            char split = '#';
            string result = content;
            for(int i = 0; i < replaceValue.Length; i++)
            {
                string replaceStr = split + index.ToString();
                result= result.Replace(replaceStr, replaceValue[i]);
                index++;
            }
            return result;
        } 
        public static GameObject CreateInstace(string objPath,GameObject parent,bool isActive)
        {
            GameObject instance = ObjectManager.Instance.InstantiateObject(objPath);
            instance.transform.SetParent(parent.transform);
            instance.SetActive(isActive);
            return instance;
        }

        public static GameObject SafeFindGameobject(string path)
        {
            GameObject obj = null;
            try
            {
                obj=GameObject.Find(path);         
            }
            catch(Exception e)
            {
                Debug.LogError(e);
            }
            return obj;
        }


    }

    public class RandomObject
    {
        public int Weight { get; set; }
    }



}