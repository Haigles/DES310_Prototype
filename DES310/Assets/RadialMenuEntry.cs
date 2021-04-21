using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenuEntry : MonoBehaviour
{
    [SerializeField]
    Text Label;

    public void SetLabel(string pText)
    {
        Label.text = pText;
    }

}
