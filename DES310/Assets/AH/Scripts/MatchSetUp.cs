using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MatchSetUp : MonoBehaviour
{
    [Header("Card Positions")]
    public Transform matchPosition;
    [SerializeField]
    public List<Transform> choicePositions = new List<Transform>();

    [Header("Card Prefab")]
    [Space(10)]
    public GameObject cardPrefab;

    [Header("Card Pool")]
    [Space(10)]
    public CardPool cardPool;
    public List<CardInfo> matchCards = new List<CardInfo>();
    public List<CardInfo> choiceCards = new List<CardInfo>();

    [HideInInspector]
    public GameObject matchCard;
    [HideInInspector]
    public List<GameObject> choices = new List<GameObject>();
    [HideInInspector]
    public bool addedCards = false;

    private GameManager manager;
    public CameraHandler cameraHandler;
    private GameObject mainCamera;

    [Header("UI Elements")]
    [Space(10)]
    public AnimalSelection animalSelection;
    public GameObject matchesPosition;
    public GameObject background;
    public GameObject topRightUIElements;
    public UpgradeEnclosure enclosurePreview;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");       
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.state == MatchState.setUp)//If game manager state is on 'SetUp' (AH)
        {
            background.SetActive(true);
            topRightUIElements.SetActive(true);

            if (!addedCards) //If haven't added cards yet (AH)
            {
                //Based on which animal was selected, add that animal's card pool (AH)
                if (animalSelection.animals[0])
                {
                    AnimalChoice(cardPool.pandaMatchCards, cardPool.pandaChoiceCards, "Panda", true);
                    enclosurePreview.panda = true;
                }
                else
                if (animalSelection.animals[1])
                {
                    AnimalChoice(cardPool.penguinMatchCards, cardPool.penguinChoiceCards, "Penguin", true);
                    enclosurePreview.penguin = true;
                }
                else
                if (animalSelection.animals[2])
                {
                    AnimalChoice(cardPool.giraffeMatchCards, cardPool.giraffeChoiceCards, "Giraffe", true);
                    //enclosurePreview.GetComponent<UpgradeEnclosure>().giraffe = true;
                }
                else
                if (animalSelection.animals[3])
                {
                    AnimalChoice(cardPool.lionMatchCards, cardPool.lionChoiceCards, "Lion", true);
                    //enclosurePreview.GetComponent<UpgradeEnclosure>().lion = true;
                }

                addedCards = true;
            }

            if (choiceCards.Count > 0 || matchCards.Count > 0)
            {
                SetUp(); //Set up cards (AH)
            }
            else
            {
                manager.state = MatchState.recap; //Changes game manager state to 'Recap' (AH)
            }
        }
    }

    private void SetUp()
    {
        cameraHandler.ChangeCameraPosMatch();
        //mainCamera.transform.position = new Vector3(50, 0, -10); //Moves main camera to the matching area (AH)

        matchCard = Instantiate(cardPrefab, matchPosition.position, Quaternion.identity); //Adds a card prefab to the scene at the match position (AH)
        CardDetails matchCardDetails = matchCard.GetComponent<CardDetails>();
        matchCard.transform.parent = matchPosition;
        matchCard.GetComponent<Draggable>().parentHome = matchCard.transform.parent;
        matchCard.transform.localPosition = new Vector2(0, 0); //Assigns the match card's parent to the match position, and sets its local position (AH)

        AddCardDetails(matchCardDetails, 1, matchCards, "MatchCard"); //Add the match's card details (AH)

        for (int i = 0; i < choicePositions.Count; i++) //for each choice position (AH)
        {
            choices.Add(Instantiate(cardPrefab, choicePositions[i].position, Quaternion.identity)); //Adds a card prefab to the scene at each choice position (AH)
            CardDetails choiceCardDetails = choices[i].GetComponent<CardDetails>();

            choices[i].transform.parent = choicePositions[i];
            choices[i].GetComponent<Draggable>().parentHome = choices[i].transform.parent;
            choices[i].transform.localPosition = new Vector2(0, 0); //Assigns the choice card's parent to the choice position, and sets its local position (AH)
            
            AddCardDetails(choiceCardDetails, choices.Count, choiceCards, "ChoiceCard"); //Add the choice's card details (AH)
        }

        manager.state = MatchState.calculate; //Changes game manager state to 'Calculate' (AH)
    }

    public void Shuffle(List<CardInfo> list) //Shuffles all of the list (AH)
    {
        for (int i = 0; i < list.Count; i++)
        {
            CardInfo temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public void AddCardDetails(CardDetails details, int list, List<CardInfo> cards, string tag) //Add all of the card details from the given card pool info to the instantiated cards (AH)
    {
        for (int i = 0; i < list; i++)
        {
            details.transform.tag = tag;
            details.nameText.GetComponent<TMP_Text>().text = cards[i].cardName;
            details.cardPicture.GetComponent<RawImage>().texture = cards[i].cardPicture;
            details.backgroundColour.GetComponent<RawImage>().color = cards[i].backgroundColour;
            details.ageText.GetComponent<TMP_Text>().text = cards[i].cardAge;
            details.maleParentText.GetComponent<TMP_Text>().text = cards[i].maleParent;
            details.femaleParentText.GetComponent<TMP_Text>().text = cards[i].femaleParent;

            if (details.transform.tag == "ChoiceCard")
            {
                details.distanceText.GetComponent<TMP_Text>().text = cards[i].cardDistance + "km";
            }
            else
            {
                details.distanceText.GetComponent<TMP_Text>().text = cards[i].cardDistance;
            }

            details.healthText.GetComponent<TMP_Text>().text = cards[i].cardHealth;
            details.valueAge = cards[i].valueAge;
            details.maleParent = cards[i].maleParent;
            details.femaleParent = cards[i].femaleParent;
            details.valueDistance = cards[i].valueDistance;
            details.valueHealth = cards[i].valueHealth;
        }
    }

    public void AnimalChoice(CardInfo[] animalMatchCards, CardInfo[] animalChoiceCards, string animalName, bool shuffle)
    {
        for (int i = 0; i < animalMatchCards.Length; i++)
        {
            matchCards.Add(animalMatchCards[i]); //adds all of the match cards from the animal's card pool (AH)
        }

        if(shuffle)
            Shuffle(matchCards); //shuffle match cards (AH)

        for (int i = 0; i < animalChoiceCards.Length; i++)
        {
            choiceCards.Add(animalChoiceCards[i]); //adds all of the choice cards from the animal's card pool (AH)
        }

        if (shuffle)
            Shuffle(choiceCards); //shuffle choice cards (AH)
    }
}
