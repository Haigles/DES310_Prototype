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
    private GameObject mainCamera;
    private bool addedCards = false;

    public GameObject enclosureCameraUI;
    public AnimalSelection animalSelection;
    public Text animalIndicator;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        enclosureCameraUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.state == MatchState.setUp)
        {
            if (!addedCards)
            {
                if (animalSelection.animals[0])
                {
                    AnimalChoice(cardPool.pandaMatchCards, cardPool.pandaChoiceCards, "Panda");
                }
                if (animalSelection.animals[1])
                {
                    AnimalChoice(cardPool.penguinMatchCards, cardPool.penguinChoiceCards, "Penguin");
                }
                if (animalSelection.animals[2])
                {
                    AnimalChoice(cardPool.giraffeMatchCards, cardPool.giraffeChoiceCards, "Giraffe");
                }
                if (animalSelection.animals[3])
                {
                    AnimalChoice(cardPool.lionMatchCards, cardPool.lionChoiceCards, "Lion");
                }

                addedCards = true;
            }

            if (choiceCards.Count > 0 || matchCards.Count > 0)
            {
                SetUp();
            }
            else
            {
                manager.state = MatchState.recap;
                //enclosureCameraUI.SetActive(false);
            }
        }
    }

    private void SetUp()
    {
        matchCard = Instantiate(cardPrefab, matchPosition.position, Quaternion.identity);
        CardDetails matchCardDetails = matchCard.GetComponent<CardDetails>();

        mainCamera.transform.position = new Vector2(50, 0);

        enclosureCameraUI.SetActive(true);

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

    public void AnimalChoice(CardInfo[] animalMatchCards, CardInfo[] animalChoiceCards, string animalName)
    {
        for (int i = 0; i < animalMatchCards.Length; i++)
        {
            matchCards.Add(animalMatchCards[i]);
        }
        Shuffle(matchCards);

        for (int i = 0; i < animalChoiceCards.Length; i++)
        {
            choiceCards.Add(animalChoiceCards[i]);
        }
        Shuffle(choiceCards);

        animalIndicator.text = animalName;
    }
}
