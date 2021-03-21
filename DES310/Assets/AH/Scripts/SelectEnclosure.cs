using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class SelectEnclosure : MonoBehaviour
{
    public GameObject plotHighlight;

    // Start is called before the first frame update
    void Start()
    {
        plotHighlight.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseOver()
    {
        plotHighlight.SetActive(true);
    }

    void OnMouseExit()
    {
        plotHighlight.SetActive(false);
    }
}
