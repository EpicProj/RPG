using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sim_FrameWork
{
    public class BlockInfoDialogContent : WindowBase
    {
        private const string DISTRICTSLOT_PREFAB_PATH = "Assets/Prefabs/Object/BlockGrid.prefab";
        private const string DISTRICTSLOT_EMPTY_IMAGE = "SpriteOutput/District/District_Empty";
        private Sprite EmptyDistrictSlotSprite;

        private BlockInfoDialog m_dialog;
        private Button clostBtn;
        FunctionBlock currentBlock;
        List<List<DistrictData>> currentDistrictData=new List<List<DistrictData>> ();

        /// <summary>
        /// [0] currentBlockid   [1] currentDistrictData
        /// </summary>
        /// <param name="paralist"></param>
        public override void Awake(params object[] paralist)
        {
            currentBlock = FunctionBlockModule.Instance.GetFunctionBlockByBlockID((int)paralist[0]);
            currentDistrictData = (List<List<DistrictData>>)paralist[1];

            EmptyDistrictSlotSprite = Utility.LoadSprite(DISTRICTSLOT_EMPTY_IMAGE, Utility.SpriteType.png);
            m_dialog = GameObject.GetComponent<BlockInfoDialog>();
            clostBtn = GameObject.Find("MainBG").GetComponent<Button>();
            AddBtnListener();
            InitDistrictDataSlot();
        }



        public override void OnShow(params object[] paralist)
        {
            //Init Text
            m_dialog.Title.transform.Find("BG2/Desc/FacotryName").GetComponent<Text>().text = FunctionBlockModule.Instance.GetFunctionBlockName(currentBlock);
            m_dialog.BlockInfoDesc.text = FunctionBlockModule.Instance.GetFunctionBlockDesc(currentBlock);
            currentBlock = FunctionBlockModule.Instance.GetFunctionBlockByBlockID((int)paralist[0]);
            //Init Sprite
            //m_dialog.FactoryBG.GetComponent<Image>().sprite = FunctionBlockModule.Instance.GetFunctionBlockIcon(currentBlock.FunctionBlockID);
        }

        private void InitDistrictDataSlot()
        {
            if (currentDistrictData == null)
                return;
            //Calculate Slot
            int line = currentDistrictData[0].Count;
            float width = 370f;
            float size =Mathf.Floor((width - m_dialog.DistrictSlotContent.GetComponent<GridLayoutGroup>().spacing.x * line) / line);
            m_dialog.DistrictSlotContent.GetComponent<GridLayoutGroup>().cellSize = new Vector2(size, size);

            for (int i = 0; i < currentDistrictData.Count; i++)
            {
                //生成行
                for(int j = 0; j < currentDistrictData[i].Count; j++)
                {
                    DistrictData data = currentDistrictData[i][j];
                    if (data.DistrictID == -1)
                    {
                        //Init EmptySlot
                        GameObject EmptySlot= ObjectManager.Instance.InstantiateObject(DISTRICTSLOT_PREFAB_PATH);
                        EmptySlot.GetComponent<Image>().sprite = EmptyDistrictSlotSprite;
                        EmptySlot.transform.Find("EmptyInfo").gameObject.SetActive(true);
                        EmptySlot.transform.SetParent(m_dialog.DistrictSlotContent.transform,false);
                    }else if (data.DistrictID == -2)
                    {
                        GameObject UnlockSlot = ObjectManager.Instance.InstantiateObject(DISTRICTSLOT_PREFAB_PATH);
                        UnlockSlot.transform.SetParent(m_dialog.DistrictSlotContent.transform, false);
                        UnlockSlot.GetComponent<Button>().interactable = false;
                    }else if (data == null)
                    {
                        Debug.LogError("Init District Slot Error!  Row=" + i + "line=" + j);
                        continue;
                    }
                    else
                    {
                        GameObject Slot = ObjectManager.Instance.InstantiateObject(DISTRICTSLOT_PREFAB_PATH);
                        GameObject district = Slot.transform.Find("District").gameObject;
                        district.gameObject.SetActive(true);
                        district.GetComponent<Image>().sprite = Utility.LoadSprite(data.DistrictIcon,Utility.SpriteType.png);
                        Slot.transform.SetParent(m_dialog.DistrictSlotContent.transform, false);
                    }
                }
            }

        }


        //Btn
        private void AddBtnListener()
        {
            AddButtonClickListener(clostBtn, delegate () { HideInfoDialog();});
        }

        private void HideInfoDialog()
        {
            UIManager.Instance.HideWnd(UIPath.FUCNTIONBLOCK_INFO_DIALOG);
        }
    }
}