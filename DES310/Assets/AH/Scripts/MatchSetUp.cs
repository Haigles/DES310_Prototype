using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchSetUp : MonoBehaviour
{
    [Space(5)]
    [Header("Card Positions")]
    public Transform matchPosition;
    [SerializeField]
    public List<Transform> choicePositions = new List<Transform>();

    [Space(5)]
    [Header("Card Prefab")]
    [Space(10)]
    public GameObject cardPrefab;

    public CardPool cardPool;
    public List<CardInfo> matchCards = new List<CardInfo>();
    public List<CardInfo> choiceCards = new List<CardInfo>();

    [HideInInspector]
    public GameObject matchCard;
    [HideInInspector]
    public List<GameObject> choices = new List<GameObject>();

    private GameManager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (manager.state == MatchState.setUp)
            {
                if (choiceCards.Count > 0 || matchCards.Count > 0)
                {
                    SetUp();
                }
                else
                {
                    manager.state = MatchState.recap;
                }
            }
        }
    }

    private void SetUp()
    {
        matchCard = Instantiate(cardPrefab, matchPosition.position, Quaternion.identity);
        CardDetails matchCardDetails = matchCard.GetComponent<CardDetails>();

        matchCard.transform.parent = matchPosition;
        matchCard.transform.localPosition = new Vector2(0, 0);

        AddCardDetails(matchCardDetails, 1, matchCards, "MatchCard");

        for (int i = 0; i < choicePositions.Count; i++)
        {
            choices.Add(Instantiate(cardPrefab, choicePositions[i].position, Quaternion.identity));
            CardDetails choiceCardDetails = choices[i].GetComponent<CardDetails>();

            choices[i].transform.parent = choicePositions[i];
            choices[i].transform.localPosition = new Vector2(0, 0);

            AddCardDetails(choiceCardDetails, choices.Count, choiceCards, "ChoiceCard");
        }

        manager.state = MatchState.calculate;

    }

    public void Shuffle(List<CardInfo> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            CardInfo temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    private void ClearCards()
    {
        if (choiceCards.Count >= 0 || matchCards.Count >= 0)
        {
            choiceCards.RemoveRange(1, 3);
            choices.Clear();
            Shuffle(choiceCards);

            matchCards.RemoveAt(0);
            Shuffle(matchCards);

            manager.state = MatchState.setUp;
        }
        else
        {
            manager.state = MatchState.recap;
        }
    }

    private void AddCardDetails(CardDetails details, int list, List<CardInfo> cards, string tag)
    {
        for (int i = 0; i < list; i++)
        {
            details.transform.tag = tag;
            details.nameText.GetComponent<Text>().text = cards[i].cardName;
            details.cardPicture.GetComponent<RawImage>().texture = cards[i].cardPicture;
            details.ageText.GetComponent<Text>().text = cards[i].cardAge;
            details.parentsText.GetComponent<Text>().text = cards[i].maleParent + " & " + cards[i].femaleParent;
            if (details.transform.tag == "ChoiceCard")
            {
                details.distanceText.GetComponent<Text>().text = cards[i].cardDistance + "km";
            }
            else
            {
                details.distanceText.GetComponent<Text>().text = cards[i].cardDistance;
            }
            details.healthText.GetComponent<Text>().text = cards[i].cardHealth;
            details.valueAge = cards[i].valueAge;
            details.maleParent = cards[i].maleParent;
            details.femaleParent = cards[i].femaleParent;
            details.valueDistance = cards[i].valueDistance;
            details.valueHealth = cards[i].valueHealth;
        }
    }
}
