using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sim_FrameWork
{
    public class Utility : MonoBehaviour
    {

        public static Sprite LoadSprite(string SpritePath)
        {
            Sprite sprite = null;
            try
            {
                sprite = ResourceManager.Instance.LoadResource<Sprite>(SpritePath);
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