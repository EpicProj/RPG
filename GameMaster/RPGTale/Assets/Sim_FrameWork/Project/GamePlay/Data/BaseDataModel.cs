using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public interface BaseDataModel
    {
        int ID { get; set; }
        bool Create(int id);
        void CleanUp();
    }
}