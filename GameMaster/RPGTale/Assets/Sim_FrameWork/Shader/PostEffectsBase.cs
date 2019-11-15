using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{

    [ExecuteInEditMode]
    public class PostEffectsBase : MonoBehaviour
    {
        protected void CheckResources()
        {
            bool isSupported = CheckSupport();

            if (isSupported == false)
            {
                NotSupported();
            }
        }

        protected bool CheckSupport()
        {
            if (SystemInfo.supportsImageEffects == false )
            {
                Debug.LogWarning("This platform does not support image effects or render textures.");
                return false;
            }

            return true;
        }

        protected void NotSupported()
        {
            enabled = false;
        }

        protected void Start()
        {
            CheckResources();
        }

        protected UnityEngine.Material CheckShaderAndCreateMaterial(Shader shader, UnityEngine.Material material)
        {
            if (shader == null)
            {
                return null;
            }

            if (shader.isSupported && material && material.shader == shader)
                return material;

            if (!shader.isSupported)
            {
                return null;
            }
            else
            {
                material = new UnityEngine.Material(shader);
                material.hideFlags = HideFlags.DontSave;
                if (material)
                    return material;
                else
                    return null;
            }
        }


    }
}