using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork {
    public class PlanetPointGenerate : MonoBehaviour
    {
        public int num;

        public float size = 10f;

        [ContextMenu("Generate")]
        public void CalculateSphere()
        {
            float inc = Mathf.PI * (3.0f - Mathf.Sqrt(5.0f));
            float off = 2.0f / num;

            for (int i = 0; i < num; i++)
            {
                float y = (float)i * off - 1.0f + (off / 2.0f);
                float r = Mathf.Sqrt(1.0f - y * y);
                float phi = i * inc;

                Vector3 pos = new Vector3(Mathf.Cos(phi) * r * size, y * size, Mathf.Sin(phi) * r * size);
                GameObject obj = new GameObject();
                obj.transform.parent = UIUtility.FindTransfrom(transform, "PointContent");
                obj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                obj.transform.localPosition = pos;
                obj.gameObject.SetActive(true);
                obj.name = "Point_" + i + "-" + pos;

            }
        }
 



    }
}