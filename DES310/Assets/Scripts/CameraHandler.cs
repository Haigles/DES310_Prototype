using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public CameraMovement cameraMovement;

    public GameObject selectPosition;
    public GameObject matchPosition;
    public GameObject startPosition;

    // Start is called before the first frame update
    void Start()
    {
        cameraMovement.SetGetCameraMovePositionFunc(() => startPosition.transform.position);
    }

    public void ChangeCameraPosSelect()
    {
        cameraMovement.SetGetCameraMovePositionFunc(() => selectPosition.transform.position);
    }

    public void ChangeCameraPosMatch()
    {
        cameraMovement.SetGetCameraMovePositionFunc(() => matchPosition.transform.position);
    }
}
