using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockInfoDialog : MonoBehaviour {

    public GameObject Title;
    public GameObject Block;
    public GameObject FactoryBG;
    public Text BlockInfoDesc;

    public GameObject DistrictSlotContent;
    public GameObject DistrictContent;
    public GameObject InfoData;
    public GameObject Processbar;

    public GameObject ManufactPanel;


    //Level Info
    public Text Level;
    public Slider LvSlider;
    public GameObject LevelValue;
    private Text Lvpercent;
    //District Build
    public GameObject BuildContent;


    void Awake()
    {
        Lvpercent = LevelValue.transform.Find("Percent").GetComponent<Text>();
    }

    public IEnumerator SliderLerp(float value, float speed)
    {
        if (value == 0)
            yield break;
        float delta = (value - LvSlider.value) / speed;
        for(int i = 0; i < speed; i++)
        {
            LvSlider.value += delta;
            yield return new WaitForSeconds(0.1f);
        }
        StopCoroutine(SliderLerp(value,speed));
        Lvpercent.text = LvSlider.value.ToString() + "%";
    }


}
