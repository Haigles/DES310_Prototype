using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Experimental.Rendering.Universal;

public class SelectEnclosure : MonoBehaviour
{
    public Light2D plotHighlight;
    public GameObject animalSelectionMenu;
    public bool canPick = true;
    private GameManager manager;
    private EnclosurePreview enclosureCamera;
    public TMP_Text animalIndicator;
    public TMP_Text stageIndicator;

    // Start is called before the first frame update
    void Start()
    {
        animalIndicator.enabled = false;
        stageIndicator.enabled = false;
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
            if (canPick)
            {
                plotHighlight.enabled = true;

                if (Input.GetMouseButtonDown(0))
                {
                    enclosureCamera.currentPlot = this.gameObject.transform;
                    manager.state = MatchState.selection;
                    animalSelectionMenu.GetComponent<AnimalSelection>().selectedEnclosure = this;
                }
            }
            else
            {
                animalIndicator.enabled = true;
                stageIndicator.enabled = true;
            }
        }
    }

    void OnMouseExit()
    {
        plotHighlight.enabled = false;
        animalIndicator.enabled = false;
        stageIndicator.enabled = false;
    }
}
