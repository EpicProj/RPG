using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sim_FrameWork
{
    public class Utility 
    {
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
                return sprite;
            }
            return sprite;
        }

        public static int TryParseInt(string str)
        {
            int result;
            if(int.TryParse(str, out result) == false)
            {
                Debug.LogError("Parse Int Error,str=" + str);
            }
            return result;
        }


        public static List<int> TryParseIntList(string str,char split)
        {
            List<int> result = new List<int> ();
            if (string.IsNullOrEmpty(str))
            {
                Debug.LogWarning("Parse Int List Error");
                return result;
            }
           
            string[] s = str.Split(split);
            for(int i = 0; i < s.Length; i++)
            {
                int n;
                if( int.TryParse(s[i], out n))
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

        public static bool CheckValueInRange(float min,float max, float value,string debug=null)
        {
            if(value<min || value > max)
            {
                Debug.LogError("Value not in range,value=" + value +"  , DebugContent=" + debug);
                return false;
            }
            return true;
        }


    }

    public class MultiDictionary<Key1, Key2, Value>
    {
        Dictionary<Key1, Dictionary<Key2, Value>> dic = new Dictionary<Key1, Dictionary<Key2, Value>>();

        public void Add(Key1 key1, Key2 key2, Value value)
        {
            if (dic.ContainsKey(key1))
            {
                var dic2 = dic[key1];
                if (dic2.ContainsKey(key2))
                {
                    dic2[key2] = value;
                }
                else
                {
                    dic2.Add(key2, value);
                }
            }
            else
            {
                var dic2 = new Dictionary<Key2, Value>();
                dic2.Add(key2, value);
                dic.Add(key1, dic2);

            }
        }
        public void Remove()
        {

        }

        public Value GetValue(Key1 key1, Key2 key2, Value defaultValue = default(Value))
        {
            if (dic.ContainsKey(key1))
            {
                var dic2 = dic[key1];
                if (dic2.ContainsKey(key2))
                {
                    return dic2[key2];

                }
            }
            return defaultValue;
        }

       

    }


}