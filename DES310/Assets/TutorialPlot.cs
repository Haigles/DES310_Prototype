using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering.Universal;

public class TutorialPlot : MonoBehaviour
{
    public Tutorial tutorial = null;
    public Light2D plotHighlight = null;
    private GameManager manager = null;

    void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        plotHighlight.enabled = false;
    }

    void OnMouseOver()
    {
        if (!PauseMenu.gameIsPaused)
        {
            plotHighlight.enabled = true;

            if (Input.GetMouseButtonDown(0))
            {
                tutorial.tutorialStage++;
            }
        }
    }
    void OnMouseExit()
    {
        plotHighlight.enabled = false;
    }
}
