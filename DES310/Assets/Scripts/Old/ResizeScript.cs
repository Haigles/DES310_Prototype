using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeScript : MonoBehaviour
{

    public void OnMouseOver()
    {
        Debug.Log("enter");
        transform.localScale = new Vector3(0.5F, 0.5f, 0.5f); //adjust these values as you see fit
    }


    public void OnMouseExit()
    {
        Debug.Log("left");
        transform.localScale = new Vector3(1, 1, 1);  // assuming you want it to return to its original size when your mouse leaves it.
    }
}