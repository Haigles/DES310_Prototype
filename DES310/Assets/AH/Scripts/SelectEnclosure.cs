using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;

public class SelectEnclosure : MonoBehaviour
{
    public Light2D plotHighlight;
    public GameObject animalSelectionMenu;
    private GameManager manager;
    private EnclosurePreview enclosureCamera;

    // Start is called before the first frame update
    void Start()
    {
        plotHighlight.enabled = false;
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        enclosureCamera = GameObject.FindGameObjectWithTag("EnclosureCamera").GetComponent<EnclosurePreview>();
        plotHighlight = this.gameObject.GetComponent<Light2D>();
        animalSelectionMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.state == MatchState.selection)
        {
            animalSelectionMenu.SetActive(true);
        }
    }

    void OnMouseOver()
    {
        if (manager.state == MatchState.hub)
        {
            plotHighlight.enabled = true;

            if (Input.GetMouseButtonDown(0))
            {
                enclosureCamera.currentPlot = this.gameObject.transform;
                manager.state = MatchState.selection;
            }
        }
    }

    void OnMouseExit()
    {
        plotHighlight.enabled = false;
    }
}
