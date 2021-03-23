using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnclosurePreview : MonoBehaviour
{
    public Transform currentPlot;
    [Range(-20, 20)]
    public float cameraSize;

    // Update is called once per frame
    void LateUpdate()
    {
        this.gameObject.GetComponent<Camera>().orthographicSize = cameraSize;

        if (currentPlot != null)
        {
            Vector3 newPos = currentPlot.position;
            newPos.z = -5;

            transform.position = newPos;
        }
    }
}
