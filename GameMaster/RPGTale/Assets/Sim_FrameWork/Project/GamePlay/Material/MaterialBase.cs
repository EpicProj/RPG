using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialBase : MonoBehaviour {

    public int MaterialID;
    public int MaterialUID;

    public virtual void InitData() { }

    public virtual void Awake() { }

    //Action
    public virtual void OnSelectMaterial() { }
    public virtual void OnHoldMaterial() { }
    public virtual void OnDestoryMaterial() { }
}
