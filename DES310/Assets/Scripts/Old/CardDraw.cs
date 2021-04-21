using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDraw : MonoBehaviour
{
    public GameObject card;
    public GameObject playerHand;
    public GameObject Matchee;
    public GameObject matched;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    

    public void Draw()
    {
        foreach (Transform child in playerHand.transform) //Clears the hand
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (Transform child in Matchee.transform) //Clears matchee
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (Transform child in matched.transform) //Clears matchee
        {
            GameObject.Destroy(child.gameObject);
        }

        GameObject matcheeCard = Instantiate(card, new Vector3(0, 0, 0), Quaternion.identity);
        matcheeCard.transform.SetParent(Matchee.transform, false);


        for (var i = 0; i < 3; i ++)  //Creates cards as child to hand area
        {
            GameObject playerCard = Instantiate(card, new Vector3(0, 0, 0), Quaternion.identity);
            playerCard.transform.SetParent(playerHand.transform, false);
        }
    }

    //TODO - Guarantee Good card from draw
}
