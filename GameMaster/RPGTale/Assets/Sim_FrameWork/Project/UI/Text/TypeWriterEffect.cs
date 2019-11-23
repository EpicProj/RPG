using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    [RequireComponent(typeof(Text))]
    public class TypeWriterEffect : MonoBehaviour
    {
        public float typeSpeed = 0.02f;
        public int CharsPerCount = 3;

        private string words;

        private bool isActive = false;
        private float timer;
        private Text m_text;
        private int currentPos = 0;


        void Awake()
        {
            timer = 0;
            isActive = true;
            m_text = GetComponent<Text>();
            words = m_text.text;
            m_text.text = "";
        }

        void Update()
        {
            StartWriter();
        }

        public void StartEffect()
        {
            Reset();
            isActive = true;
        }
       
        void StartWriter()
        {
            if (isActive)
            {
                timer += Time.deltaTime;
                if (timer >= typeSpeed)
                {
                    timer = 0;
                    currentPos+= CharsPerCount;
                    if (currentPos > words.Length)
                        currentPos = words.Length;
                    m_text.text = words.Substring(0, currentPos);

                    if (currentPos >= words.Length)
                    {
                        Finish();
                    }
                }
            }
        }

        void Finish()
        {
            isActive = false;
            timer = 0;
            currentPos = 0;
            m_text.text = words;
        }

        void Reset()
        {
            words = m_text.text;
            m_text.text = "";
            timer = 0;
            currentPos = 0;
        }
    }
}