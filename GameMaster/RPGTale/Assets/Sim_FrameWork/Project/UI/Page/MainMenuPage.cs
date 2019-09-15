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
    //Resource
    public GameObject ResouceContent;
    public GameObject Currency;
    public GameObject Food;
    public GameObject Energy;
    public GameObject Labor;
    public Button MaterialBtn;

    //BuildPanel
    public GameObject BuildTabContent;
    public GameObject BuildSubTabContent;

    //Camp
    public GameObject CampContent;
    public GameObject CampValue;

    //Btn
    [Header("Button")]
    public Button ConstructBtn;
    public Button RoadBtn;
    public Button OrderBtn;
    public Button ReserachBtn;
    public Button EventsBtn;
        
}
