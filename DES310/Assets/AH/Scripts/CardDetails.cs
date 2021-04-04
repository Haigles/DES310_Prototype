using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDetails : MonoBehaviour
{
    public GameObject cardPicture, nameText, ageText, parentsText, distanceText, healthText; //Card Text Fields (Children of Prefab) (AH)
    public int valueAge, valueDistance, valueHealth; //Values of details (AH)
    public string maleParent, femaleParent; //Names of the card's parents (AH)
    public int cardTotalValue = 0; //Value used to calculate matches (AH)
}
