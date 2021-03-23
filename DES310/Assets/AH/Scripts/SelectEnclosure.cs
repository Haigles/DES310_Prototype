using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class SelectEnclosure : MonoBehaviour
{
    public Light2D plotHighlight;
    private GameManager manager;
    private EnclosurePreview enclosureCamera;

    // Start is called before the first frame update
    void Start()
    {
        plotHighlight.enabled = false;
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        enclosureCamera = GameObject.FindGameObjectWithTag("EnclosureCamera").GetComponent<EnclosurePreview>();
        plotHighlight = this.gameObject.GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseOver()
    {
        plotHighlight.enabled = true;

        if (Input.GetMouseButtonDown(0))
        {
            if (manager.state == MatchState.hub)
            {
                enclosureCamera.currentPlot = this.gameObject.transform;
                manager.state = MatchState.setUp;
            }
        }
    }

    void OnMouseExit()
    {
        plotHighlight.enabled = false;
    }
}
