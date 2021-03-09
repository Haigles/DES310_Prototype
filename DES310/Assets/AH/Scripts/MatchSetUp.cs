using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchSetUp : MonoBehaviour
{
    [Space(5)]
    [Header("Card Positions")]
    public GameObject matchPosition;
    public GameObject choice1Position;
    public GameObject choice2Position;
    public GameObject choice3Position;

    [Space(5)]
    [Header("Card Prefab")]
    [Space(10)]
    public GameObject cardPrefab;

    public CardPool cardPool;
    public List<CardInfo> matchCards = new List<CardInfo>();
    public List<CardInfo> choiceCards = new List<CardInfo>();

    private bool hasSetUp = false;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < cardPool.pandaMatchCards.Length; i++)
        {
            matchCards.Add(cardPool.pandaMatchCards[i]);
        }
        Shuffle(matchCards);

        for (int i = 0; i < cardPool.pandaChoiceCards.Length; i++)
        {
            choiceCards.Add(cardPool.pandaChoiceCards[i]);
        }
        Shuffle(choiceCards);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetUp();
        }
    }

    private void SetUp()
    {
        if (!hasSetUp)
        {
            GameObject matchCard = Instantiate(cardPrefab, matchPosition.transform.position, Quaternion.identity);
            CardDetails matchCardDetails = matchCard.GetComponent<CardDetails>();

            matchCard.transform.parent = matchPosition.transform;
            matchCard.transform.localPosition = new Vector2(0, 0);

            matchCardDetails.nameText.GetComponent<Text>().text = matchCards[0].cardName;
            matchCardDetails.ageText.GetComponent<Text>().text = matchCards[0].cardAge;
            matchCardDetails.parentsText.GetComponent<Text>().text = matchCards[0].maleParent + " & " + matchCards[0].femaleParent;
            matchCardDetails.locationText.GetComponent<Text>().text = matchCards[0].cardLocation;
            matchCardDetails.healthText.GetComponent<Text>().text = matchCards[0].cardHealth;

            hasSetUp = true;
        }
    }

    private void Shuffle(List<CardInfo> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            CardInfo temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
