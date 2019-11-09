using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class AudioClipPath
    {
        private const string EffectBasePath = "Assets/Audio/Effect/";

        public struct UISound
        {
            public const string Btn_Close = EffectBasePath + "UI/Btn_Close.wav";
        }

        public struct ItemEffect
        {
            public const string Block_Move = EffectBasePath + "Item/Block_Move.wav";
            public const string Block_Place = EffectBasePath + "Item/Block_Place.wav";
            public const string Block_Select = EffectBasePath + "Item/Block_Select.wav";
        }
       
      
    }
}