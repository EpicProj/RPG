using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class Test : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            Config.JsonReader reader = new Config.JsonReader();
            reader.LoadMaterialRarityDataConfig();
            
        }


    }
}