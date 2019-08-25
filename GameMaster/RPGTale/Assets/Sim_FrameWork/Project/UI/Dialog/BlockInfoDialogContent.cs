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
        FunctionBlock_Info blockInfo;

        /// <summary>
        /// [0] currentBlockid   [1] currentDistrictData
        /// </summary>
        /// <param name="paralist"></param>
        public override void Awake(params object[] paralist)
        {
            blockInfo = (FunctionBlock_Info)paralist[0];

            EmptyDistrictSlotSprite = Utility.LoadSprite(DISTRICTSLOT_EMPTY_IMAGE, Utility.SpriteType.png);
            m_dialog = GameObject.GetComponent<BlockInfoDialog>();
            clostBtn = GameObject.Find("MainBG").GetComponent<Button>();
            AddBtnListener();
            InitDistrictDataSlot();
        }



        public override void OnShow(params object[] paralist)
        {
            //Init Text
            m_dialog.Title.transform.Find("BG2/Desc/FacotryName").GetComponent<Text>().text = FunctionBlockModule.Instance.GetFunctionBlockName(blockInfo.block);
            m_dialog.BlockInfoDesc.text = FunctionBlockModule.Instance.GetFunctionBlockDesc(blockInfo.block);
            blockInfo = (FunctionBlock_Info)paralist[0];
            //Init Sprite
            //m_dialog.FactoryBG.GetComponent<Image>().sprite = FunctionBlockModule.Instance.GetFunctionBlockIcon(currentBlock.FunctionBlockID);
        }

        //生成初始区划格
        private void InitDistrictDataSlot()
        {
            if (blockInfo.currentDistrictDataDic == null)
                return;
            //Calculate Slot
            int line = (int)blockInfo.districtAreaMax.x;
            float width = 370f;
            float size = Mathf.Floor((width - m_dialog.DistrictSlotContent.GetComponent<GridLayoutGroup>().spacing.x * line) / line);
            m_dialog.DistrictSlotContent.GetComponent<GridLayoutGroup>().cellSize = new Vector2(size, size);

            foreach (KeyValuePair<Vector2, DistrictAreaInfo> kvp in blockInfo.currentDistrictDataDic)
            {
                var data = kvp.Value.data;
                if (data.DistrictID == -1)
                {
                    //Init EmptySlot
                    InitEmptyDisBlock();
                }
                else if (data.DistrictID == -2)
                {
                    //Init UnlockSlot
                    InitUnlockDisBlock();
                }
                else if (kvp.Value.isLargeDistrict == true )
                {
                    List<Vector2> areaList = kvp.Value.largeDistrictIndex;
                    Vector2 maxArea = DistrictModule.Instance.FindMaxDistrictBlockVector(kvp.Value);

                    if (areaList.Contains(kvp.Key) && !kvp.Key.Equals(maxArea))
                    {
                        //Skip Slot
                        InitEmptyDisBlock();

                    }else if (areaList.Contains(kvp.Key) && kvp.Key.Equals(maxArea))
                    {
                        //Init LargeBlock
                        InitLargeDisBlock(data,(int)size);
                    }
                }
                else if (kvp.Value.isLargeDistrict == false)
                {
                    InitSmallDisBlock(data);
                }
                else
                {
                    Debug.LogError("Init District Slot Error! key=" + kvp.Key);
                    continue;
                }
            }
        }

        private void UpdateDistrictSlot()
        {

        }


        public void InitLargeDisBlock(DistrictData data ,int size)
        {
            GameObject Slot = ObjectManager.Instance.InstantiateObject(DISTRICTSLOT_PREFAB_PATH);
            GameObject district = Slot.transform.Find("District").gameObject;
            district.gameObject.SetActive(true);
            district.GetComponent<Image>().sprite = Utility.LoadSprite(data.DistrictIcon, Utility.SpriteType.png);
            //Set Size
            Vector2 v = DistrictModule.Instance.GetDistrictArea(data);
            district.GetComponent<RectTransform>().sizeDelta = new Vector2(v.y * size, v.x * size);
            district.transform.Find("Name").GetComponent<Text>().text = DistrictModule.Instance.GetDistrictName(data);
            Slot.transform.SetParent(m_dialog.DistrictSlotContent.transform, false);
        }


        public void InitSmallDisBlock(DistrictData data)
        {
            GameObject Slot = ObjectManager.Instance.InstantiateObject(DISTRICTSLOT_PREFAB_PATH);
            GameObject district = Slot.transform.Find("District").gameObject;
            district.gameObject.SetActive(true);
            district.GetComponent<Image>().sprite = Utility.LoadSprite(data.DistrictIcon, Utility.SpriteType.png);
            district.transform.Find("Name").GetComponent<Text>().text = DistrictModule.Instance.GetDistrictName(data);

            Slot.transform.SetParent(m_dialog.DistrictSlotContent.transform, false);
        }
        public void InitEmptyDisBlock()
        {
            GameObject EmptySlot = ObjectManager.Instance.InstantiateObject(DISTRICTSLOT_PREFAB_PATH);
            EmptySlot.GetComponent<Image>().sprite = EmptyDistrictSlotSprite;
            EmptySlot.transform.Find("EmptyInfo").gameObject.SetActive(true);
            EmptySlot.transform.SetParent(m_dialog.DistrictSlotContent.transform, false);
        }
        public void InitUnlockDisBlock()
        {
            GameObject UnlockSlot = ObjectManager.Instance.InstantiateObject(DISTRICTSLOT_PREFAB_PATH);
            UnlockSlot.transform.SetParent(m_dialog.DistrictSlotContent.transform, false);
            UnlockSlot.GetComponent<Button>().interactable = false;
        }


        //Button
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