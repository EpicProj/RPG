using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sim_FrameWork
{
    public class Utility : MonoBehaviour
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

    }
}