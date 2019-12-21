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
            public const string Hint_Open = EffectBasePath + "UI/Hint_Open.mp3";
            public const string Button_Click = EffectBasePath + "UI/Button_Click.mp3";
            public const string Page_Open = EffectBasePath + "UI/Page_Open.mp3";
            public const string Button_General = EffectBasePath + "UI/Button_General.mp3";
        }

        public struct ItemEffect
        {
            public const string Block_Move = EffectBasePath + "Item/Block_Move.wav";
            public const string Block_Place = EffectBasePath + "Item/Block_Place.wav";
            public const string Block_Select = EffectBasePath + "Item/Block_Select.wav";
        }
       
      
    }
}