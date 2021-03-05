using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class Card
{
    public string cardName;
    public int age;
    public string sex;
    public int weight;
    public string parents;
    public string health;
    public string behaviour;

    public Sprite thisImage;

    public Card()
    {

    }
    public Card(string CardName, int Age, string Sex, int Weight, string Parents, string Health, string Behaviour, Sprite ThisImage)
    {
        cardName = CardName;
        age = Age;
        sex = Sex;
        weight = Weight;
        parents = Parents;
        health = Health;
        behaviour = Behaviour;

        thisImage = ThisImage;
    }
}
