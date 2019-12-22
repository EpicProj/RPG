using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class SolarSystemRotate : MonoBehaviour
    {
        public Transform Center;

        public float rotationAroundPlanetSpeed;
        public float rotationAroundSunDays;
        public float defaultEarthYear = 365;
        public float satelliteOrbitDistance;
        public float planetSunDistance;
        public float planetSpeedRotation;

        void Start()
        {
            rotationAroundPlanetSpeed = rotationAroundSunDays / defaultEarthYear;
        }

        void Update()
        {
            transform.RotateAround(Center.position, Vector3.up, Time.deltaTime * (defaultEarthYear / rotationAroundSunDays) * 1.0f * Time.deltaTime);
            transform.Rotate(-Vector3.up * Time.deltaTime * planetSpeedRotation * 1.0f);
        }


    }
}