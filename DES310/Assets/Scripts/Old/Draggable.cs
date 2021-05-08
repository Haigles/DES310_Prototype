using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Transform parentHome = null;
    public Transform dropPosition = null;
    private Transform initialParent = null;

    private GameObject canvas = null;

    public bool dragging = false;
    public bool inDropZone = false;

    private Vector3 initialScale;
    public float scaleValueX;
    public float scaleValueY;

    void Start()
    {
        dropPosition = GameObject.FindGameObjectWithTag("Drop").transform;
        canvas = GameObject.FindGameObjectWithTag("Canvas");

        initialParent = parentHome;
        initialScale = transform.localScale;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Drop")
        {
            parentHome = dropPosition.transform;
            inDropZone = true;
            Debug.Log("Drop Me");
        }
        else
        {
            parentHome = initialParent;
            inDropZone = false;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Drop")
        {
            parentHome = initialParent;
            inDropZone = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (transform.tag == "ChoiceCard")
        {
            transform.localScale = new Vector3(scaleValueX, scaleValueY, initialScale.z);
            this.transform.parent.SetAsLastSibling();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = initialScale;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("Drag Begin");
        //if (dragging)
        //{
        //    parentHome = this.transform.parent;
        //    this.transform.SetParent(this.transform.parent.parent);

        ////  GetComponent<CanvasGroup>().blocksRaycasts = false;
        //}
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging");
        dragging = true;
        transform.localScale = initialScale;
        if (this.transform.tag == "ChoiceCard")
        {
            this.transform.position = eventData.pointerCurrentRaycast.worldPosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Drag End");
        this.transform.position = parentHome.position;
        this.transform.SetParent(parentHome);
        //GetComponent<CanvasGroup>().blocksRaycasts = true;
        dragging = false;
    }
}
