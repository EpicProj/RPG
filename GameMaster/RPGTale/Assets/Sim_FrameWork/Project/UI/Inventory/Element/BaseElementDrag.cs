using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sim_FrameWork
{
    public class BaseElementDrag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public virtual void OnBeginDrag(PointerEventData eventData) { }
        public virtual void OnDrag(PointerEventData eventData) { }
        public virtual void OnEndDrag(PointerEventData eventData) { }

    }
}