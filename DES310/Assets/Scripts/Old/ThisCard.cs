using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ThisCard : MonoBehaviour
{
    public List<Card> thisCard = new List<Card>();
    public int thisId;

    public string cardName;
    public int age;
    public string sex;
    public int weight;
    public string parents;
    public string health;
    public string behaviour;
    

    public Text nameText;
    public Text ageText;
    public Text sexText;
    public Text weightText;
    public Text parentsText;
    public Text healthText;
    public Text behaviourText;

    public Sprite thisSprite;
    public Image thatImage;




    // Start is called before the first frame update
    int Start()
    {
        thisId = Random.Range(1, 5); //Randomises card id from CardDatabase
        thisCard[0] = CardDataBase.cardList[thisId];
        return thisId;
    }

    // Update is called once per frame
    void Update()
    {

        cardName = thisCard[0].cardName;
        age = thisCard[0].age;
        sex = thisCard[0].sex;
        weight =  thisCard[0].weight;
        parents = thisCard[0].parents;
        health = thisCard[0].health;
        behaviour = thisCard[0].behaviour;
        thisSprite = thisCard[0].thisImage;

        nameText.text = "" + cardName;
        ageText.text = "" + age;
        sexText.text = "" + sex;
        weightText.text = "" + weight;
        parentsText.text = "" + parents;
        healthText.text = "" + health;
        behaviourText.text = "" + behaviour;

        thatImage.sprite = thisSprite;
    }
}
