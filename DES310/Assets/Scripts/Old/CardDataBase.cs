using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDataBase : MonoBehaviour
{
    public static List<Card> cardList = new List<Card>();

    private void Awake()
    {
        //Information that can be given to certain card

        //public Card(string CardName, int Age, string Sex, int Weight, string Parents, string Health, string Behaviour)
        //TODO - Add value to determine how good of a match the card is

        cardList.Add(new Card("None", 0, "None", 0, "None", "None", "None", Resources.Load<Sprite>("Panda1")));
        cardList.Add(new Card("Harry", 10, "Male", 50, "Mum and Dad", "Good", "Friendly", Resources.Load<Sprite>("Panda1")));
        cardList.Add(new Card("Larry", 20, "Male", 100, "Ma and Pa", "Okay", "Timid", Resources.Load<Sprite>("Panda2")));
        cardList.Add(new Card("Gary", 30, "Male", 150, "Mother and Father", "Bad", "Aggressive", Resources.Load<Sprite>("Panda3")));
        cardList.Add(new Card("Barry", 40, "Male", 200, "Unknown", "Fine", "Lazy", Resources.Load<Sprite>("Panda4")));

        

    }
}
