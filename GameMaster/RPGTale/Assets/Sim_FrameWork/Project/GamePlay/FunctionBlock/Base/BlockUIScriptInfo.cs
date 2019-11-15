using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class BlockUIScriptInfo : MonoBehaviour
    {

        private FunctionBlockBase _baseBlock;
        [HideInInspector]
        public SelectionUIObject selectionUIInstance;

        void Start()
        {

        }

        public void SetData(FunctionBlockBase baseBlock)
        {
            _baseBlock = baseBlock;
        }

        public void ShowSelectionUI(bool show)
        {
            if (show)
            {
                if (selectionUIInstance == null)
                {
                    selectionUIInstance=UIUtility.SafeGetComponent<SelectionUIObject>(ShowUI(UIPath.Misc.Block_Selection_UI).transform); 
                }
            }
            else
            {
                if (selectionUIInstance != null)
                {
                    ObjectManager.Instance.ReleaseObject(selectionUIInstance.gameObject);
                    selectionUIInstance = null;
                }
            }
        }


        public GameObject ShowUI(string Path)
        {
            var instance = Utility.CreateInstace(Path, _baseBlock.UIRoot, true);
            instance.transform.localPosition = Vector3.zero;
            return instance;
        }

    }
}