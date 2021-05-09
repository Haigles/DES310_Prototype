using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorImage : MonoBehaviour
{

    public Sprite click;
    public Sprite drag;

    void Start()
    {
        Cursor.visible = false; //hides basic mouse cursor (AH)
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = cursorPos; //object has same position as basic mouse cursor (AH)

        if(Input.GetMouseButton(0))
        {
            this.GetComponentInChildren<SpriteRenderer>().sprite = drag;
        }
        else
        {
            this.GetComponentInChildren<SpriteRenderer>().sprite = click;
        }
    }
}
