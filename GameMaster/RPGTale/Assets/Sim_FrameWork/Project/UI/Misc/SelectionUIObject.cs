using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class SelectionUIObject : MonoBehaviour
    {
        public enum PlaceState
        {
            CanPlace,
            CanNotPlace
        }

        private Transform ArrowRight;
        private Transform ArrowLeft;
        private Transform ArrowTop;
        private Transform ArrowBottom;

        private SpriteRenderer Grid;

        private Sprite CanPlaceSprite;
        private Sprite CannotPlaceSprite;

        void Start()
        {
            ArrowRight = UIUtility.FindTransfrom(transform, "ArrowObj/Right");
            ArrowLeft = UIUtility.FindTransfrom(transform, "ArrowObj/Left");
            ArrowTop = UIUtility.FindTransfrom(transform, "ArrowObj/Top");
            ArrowBottom = UIUtility.FindTransfrom(transform, "ArrowObj/Bottom");

            Grid = UIUtility.SafeGetComponent<SpriteRenderer>(UIUtility.FindTransfrom(transform, "Grid"));
            CanPlaceSprite = Utility.LoadSprite("SpriteOutput/Map/grid_green", Utility.SpriteType.png);
            CannotPlaceSprite= Utility.LoadSprite("SpriteOutput/Map/grid_red", Utility.SpriteType.png);
        }

        public void ShowGrid(bool show)
        {
            Grid.gameObject.SetActive(show);
        }

        public void SetGridColor(PlaceState placeState)
        {
            switch (placeState)
            {
                case PlaceState.CanPlace:
                    Grid.sprite = CanPlaceSprite;
                    break;
                case PlaceState.CanNotPlace:
                    Grid.sprite = CannotPlaceSprite;
                    break;
                default:
                    break; 
            }
        }


    }
}