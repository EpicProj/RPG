using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork.UI
{
    public class BlockManufactSlotPanel : InventoryBase {




        public override void Awake()
        {
            base.Awake();
            InitData();
        }

        public void InitManuInputMaterialSlot(Material ma,int amount)
        {
            foreach(BaseElement slot in _elementList)
            {
                ManuSlotElement materialSlot = (ManuSlotElement)slot;
                if(materialSlot.SlotType == FormulaModule.MaterialProductType.Input)
                {
                    if (materialSlot.transform.childCount > 0)
                    {
                        //已经有材料
                        SlotItem currentItem = materialSlot.transform.GetChild(0).GetComponent<SlotItem>();
                        currentItem.SetMaterialData(ma, amount);
                        break;
                    }
                    else
                    {
                        materialSlot.InitManuMaterialSlot(ma, amount);
                        break;
                    }
                }
            }
        }
        public void InitManuOutputMaterialSlot(Material ma,int amount)
        {
            foreach(BaseElement slot in _elementList)
            {
                ManuSlotElement materialSlot = (ManuSlotElement)slot;
                if (materialSlot.SlotType == FormulaModule.MaterialProductType.Output)
                {
                    if (materialSlot.transform.childCount > 0)
                    {
                        //已经有材料
                        SlotItem currentItem = materialSlot.transform.GetChild(0).GetComponent<SlotItem>();
                        currentItem.SetMaterialData(ma, amount);
                        break;
                    }
                    else
                    {
                        materialSlot.InitManuMaterialSlot(ma, amount);
                        break;
                    }
                }
            }
        }


    }
}