using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalObjCheck : MonoBehaviour
{
    private void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("Global").Length > 1)
            Destroy(this.gameObject);
        else
            DontDestroyOnLoad(this.gameObject);
    }
}
