using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPage : MonoBehaviour {

    public GameObject BuildContent;
    //Time
    public GameObject TimePanel;
    public Slider TimeSlider;

    //GameStates
    public GameObject GameStatesObj;

    [Header("Resources")]
    public Transform ResourcePanel;


    //BuildPanel
    public GameObject BuildTabContent;
    public GameObject BuildSubTabContent;


    //Btn
    [Header("Button")]
    public Button OrderBtn;
    public Button ReserachBtn;
    public Button MenuBtn;
    public Button ExploreBtn;
        
}
