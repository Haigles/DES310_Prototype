using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Experimental.Rendering.Universal;

public class SelectEnclosure : MonoBehaviour //Script goes onto every 'Plot' (AH)
{
    [Space(5)]
    public bool canPick = true; //checks if plot has been populated already (AH)
    public Vector3 newOffset = new Vector3();

    [Header("Scene Canvas' Selection_Menu")]
    [Space(5)]
    public GameObject animalSelectionMenu; //in 'Canvas' in Scene Hierarchy (AH)

    [Header("This Enclosure's Light2D")]
    [Space(5)]
    public Light2D plotHighlight; //this plot's Light2D Script (AH)

    [Header("TMP Children of This Enclosure")]
    [Space(5)]
    public GameObject stageEnclosurePosition;

    private GameManager manager;

    // Start is called before the first frame update
    void Start()
    {
        animalSelectionMenu.SetActive(false);
        plotHighlight.enabled = false; //starts with all UI elements disabled (AH)

        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.state == MatchState.selection) //If game manager state in on 'Selection' (AH)
        {
            animalSelectionMenu.SetActive(true); //Bring up selection menu (AH)
        }
    }

    void OnMouseOver()
    {
        if (!PauseMenu.gameIsPaused)
        {
            if (manager.state == MatchState.hub) //If game manager state in on 'Hub' (AH)
                {
                if (canPick) //If enclosure hasn't been populated yet (AH)
                {
                    plotHighlight.enabled = true; //highlight plot (AH)

                    if (Input.GetMouseButtonDown(0)) //If LMB is down (AH)
                    {               
                        animalSelectionMenu.GetComponent<AnimalSelection>().selectedEnclosure = this; //This enclosure is assigned to the selected enclosure in the animal selection menu (AH)
                        manager.state = MatchState.selection; //Changes game manager state to 'Selection' (AH)
                    }
                }
            }
        }
    }

    void OnMouseExit() //hide all of the UI elements (AH)
    {
        plotHighlight.enabled = false;
    }
}
