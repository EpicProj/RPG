using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class ChunkExtention : MonoBehaviour
    {
        void Awake()
        {
            if (GetComponent<MeshRenderer>() == null)
            {
                gameObject.layer = 26;
            }
        }

    }
}