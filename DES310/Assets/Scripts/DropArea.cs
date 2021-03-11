using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropArea : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public CheckMatch matchee;
    //TODO: End on Drop
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Pointer Enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        matchee.GetComponent<CheckMatch>().MatchCheck();
    }
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.name + " dropped in " + gameObject.name);

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        
        if(d !=null)
        {
            d.parentHome = this.transform;
        }
        

    }
}
