using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeScript : MonoBehaviour
{

    public void OnMouseOver()
    {
        Debug.Log("enter");
        transform.localScale += new Vector3(1.5F, 1.5f, 1.5f); //adjust these values as you see fit
    }


    public void OnMouseExit()
    {
        Debug.Log("left");
        transform.localScale = new Vector3(1, 1, 1);  // assuming you want it to return to its original size when your mouse leaves it.
    }
}