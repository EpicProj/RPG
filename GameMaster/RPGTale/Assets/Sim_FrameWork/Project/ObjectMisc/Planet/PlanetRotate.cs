using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class PlanetRotate : MonoBehaviour
    {
        private Transform trans;
        public float RotateSpeed = 0.0f;
        void Start()
        {
            trans = this.transform;
        }

        void Update()
        {
            trans.Rotate(Vector3.up * RotateSpeed);
        }
    }
}