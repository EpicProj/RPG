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
            FunctionBlockBase blockBase = GetComponentInParent<FunctionBlockBase>();
            var maxSize = blockBase.info.districtAreaMax;

            ArrowRight = UIUtility.FindTransfrom(transform, "ArrowObj/Right");
            ArrowLeft = UIUtility.FindTransfrom(transform, "ArrowObj/Left");
            ArrowTop = UIUtility.FindTransfrom(transform, "ArrowObj/Top");
            ArrowBottom = UIUtility.FindTransfrom(transform, "ArrowObj/Bottom");

            Grid = UIUtility.SafeGetComponent<SpriteRenderer>(UIUtility.FindTransfrom(transform, "Grid"));
            CanPlaceSprite = Utility.LoadSprite("SpriteOutput/Map/grid_green", Utility.SpriteType.png);
            CannotPlaceSprite= Utility.LoadSprite("SpriteOutput/Map/grid_red", Utility.SpriteType.png);

            ArrowRight.localPosition =new Vector3(ArrowRight.localPosition.x-( maxSize.y / 2), 0,ArrowRight.localPosition.z);
            ArrowLeft.localPosition = new Vector3(ArrowLeft.localPosition.x + (maxSize.y / 2), 0, ArrowLeft.localPosition.z);

            ArrowTop.localPosition = new Vector3(ArrowTop.localPosition.x, 0, ArrowTop.localPosition.z - (maxSize.x / 2));
            ArrowBottom.localPosition = new Vector3(ArrowBottom.localPosition.x, 0, ArrowBottom.localPosition.z + (maxSize.x / 2));

            Grid.size = maxSize;
            Grid.transform.localPosition = new Vector3(0, -transform.localScale.y / 2, 0);
            ShowGrid(false);

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