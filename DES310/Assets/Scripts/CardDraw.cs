using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDraw : MonoBehaviour
{
    public GameObject card;
    public GameObject playerHand;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnClick()
    {
        foreach (Transform child in playerHand.transform) //Clears the hand
        {
            GameObject.Destroy(child.gameObject);
        }

        for (var i = 0; i < 3; i ++)  //Creates cards as child to hand area
        {
            GameObject playerCard = Instantiate(card, new Vector3(0, 0, 0), Quaternion.identity);
            playerCard.transform.SetParent(playerHand.transform, false);
        }
    }

    //TODO - Guarantee Good card from draw
}
