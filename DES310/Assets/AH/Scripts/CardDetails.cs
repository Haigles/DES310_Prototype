using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDetails : MonoBehaviour
{
    public GameObject backgroundColour, cardPicture, nameText, ageText, maleParentText, femaleParentText, distanceText, healthText; //Card Text Fields (Children of Prefab) (AH)
    public int valueAge, valueDistance, valueHealth; //Values of details (AH)
    public string maleParent, femaleParent; //Names of the card's parents (AH)
    public int cardTotalValue = 0; //Value used to calculate matches (AH)

    [HideInInspector]
    public List<int> values = new List<int>();

    //[HideInInspector]
    public int matchValue = 0;
    //[HideInInspector]
    public bool highAge = false, highLocation = false, highHealth = false, highParents = false;
}
