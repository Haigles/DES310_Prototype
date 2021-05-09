using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubHandler : MonoBehaviour
{
    private GameManager manager = null;
    private CameraHandler cameraHandler = null;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        cameraHandler = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CameraHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.state == MatchState.hub)
        {
            cameraHandler.ChangeCameraPosSelect();
        }
    }
}
