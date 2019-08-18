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
            PlayerModule.Instance.InitData();
            //PlayerModule.Instance.Food = PlayerModule.Instance.Food+300;
            //Debug.Log(PlayerModule.Instance.Food);
            ModifierModule.Instance.InitData();
            Debug.Log(PlayerModule.Instance.Currency);
            ModifierModule.Instance.OnAddModifier("AddCurrency");
            Debug.Log(PlayerModule.Instance.Currency);


        }


    }
}