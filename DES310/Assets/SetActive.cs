using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActive : MonoBehaviour
{
    public GameObject StartUIButtons;

    public void Enable()
    {
        StartUIButtons.SetActive(true);
    }
}
