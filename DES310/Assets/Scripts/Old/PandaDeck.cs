using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PandaDeck : MonoBehaviour
{
    public List<Card> pandaDeck = new List<Card>();
    public List<Card> container = new List<Card>();

    public int x;
    public int deckSize;

    public GameObject cardInDeck1;
    public GameObject cardInDeck2;
    public GameObject cardInDeck3;
    public GameObject cardInDeck4;

    void Start()
    {
        x = 0;
        //deckSize = 10;

        for(int i=0; i<deckSize; i++)
        {
            x = Random.Range(1, 5);
            pandaDeck[i] = CardDataBase.cardList[x];
        }
    }


    void Update()
    {
        if(deckSize<30)
        {
            cardInDeck1.SetActive(false);
        }
        if (deckSize < 20)
        {
            cardInDeck2.SetActive(false);
        }
        if (deckSize < 2)
        {
            cardInDeck3.SetActive(false);
        }
        if (deckSize < 1)
        {
            cardInDeck4.SetActive(false);
        }
    }

    public void Shuffle()
    {
        for (int i = 0; i <deckSize; i++)
        {
            container[0] = pandaDeck[i];
            int randomIndex = Random.Range(i, deckSize);
            pandaDeck[i] = pandaDeck[randomIndex];
            pandaDeck[randomIndex] = container[0];
        }
    }
}
