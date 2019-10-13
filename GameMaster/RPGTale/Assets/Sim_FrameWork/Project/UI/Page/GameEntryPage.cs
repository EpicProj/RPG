using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork.UI
{
    public class GameEntryPage : MonoBehaviour
    {
        [Header("Button")]
        public Button StartButton;
        public Button LoadButton;
        public Button SetButton;
        public Button QuitButton;

        [Header("Root")]
        public GameObject Menu;

        private const float menuAnimWaitSeconds = 2.0f;
        private bool firstShow = true;
        public Animation MenuAnim;

        void Start()
        {
            if (firstShow)
            {
                StartCoroutine(WaitMenuAnimAndPlay());
                firstShow = false;
            }
        }

        IEnumerator WaitMenuAnimAndPlay()
        {
            yield return new WaitForSeconds(menuAnimWaitSeconds);
            if (MenuAnim != null)
            {
                Menu.gameObject.SetActive(true);
                MenuAnim.Play();
            }
         
        }
    }
}