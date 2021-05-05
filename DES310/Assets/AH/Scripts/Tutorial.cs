using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Tutorial : MonoBehaviour
{
    private GameManager manager = null;
    private MatchSetUp matchSetUp = null;

    private int score = 0;
    private bool dialogueUp = true;
    private bool hasAddedCards = false;
    private bool hasClearedCards = false;
    private GameObject bestPosition = null;

    private GameObject matchCard = null;
    private List<GameObject> choiceCard = new List<GameObject>();
    private UpgradeEnclosure enclosureUpgrade = null;
   
    public bool canContinue = true;
    public int tutorialStage = 0;

    [Header("Hidden Elements")]
    [SerializeField]
    List<GameObject> hideUIElements = new List<GameObject>();
    [Space(10)]

    [Header("Card Positions")]
    public Transform dropPosition;
    public Transform matchPosition;
    [SerializeField]
    public List<Transform> choicePositions = new List<Transform>();
    [Space(10)]

    [SerializeField]
    CardPool cardPool;
    List<CardInfo> matchCards = new List<CardInfo>();
    List<CardInfo> choiceCards = new List<CardInfo>();

    [SerializeField]
    GameObject cardPrefab;

    [SerializeField]
    GameObject tutorialScreen;

    [SerializeField]
    GameObject enclosurePreview;

    [SerializeField]
    TMP_Text scoreText;

    [SerializeField]
    GameObject dialgoueUIElements;

    [SerializeField]
    GameObject basicPlot;

    [SerializeField]
    Button nextButton;

    [SerializeField]
    TMP_Text dialogueText;

    [SerializeField]
    [TextArea]
    List<string> dialogueStrings = new List<string>();

    private bool tryAgain;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        matchSetUp = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MatchSetUp>();
        enclosureUpgrade = enclosurePreview.GetComponent<UpgradeEnclosure>();

        for (int i = 0; i < cardPool.tutorialMatchCards.Length; i++)
        {
            matchCards.Add(cardPool.tutorialMatchCards[i]);
        }
        for (int i = 0; i < cardPool.tutorialChoiceCards.Length; i++)
        {
            choiceCards.Add(cardPool.tutorialChoiceCards[i]);
        }

        basicPlot.SetActive(false);
        enclosurePreview.SetActive(false);
        scoreText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.state == MatchState.tutorial)
        {
            tutorialScreen.SetActive(true);
            scoreText.text = "Score: " + score;

            for (int i = 0; i < hideUIElements.Count; i++)
            {
                hideUIElements[i].SetActive(false);
            }

            TutorialStages();
        }
        else
        {
            tutorialScreen.SetActive(false);
            for (int i = 0; i < hideUIElements.Count; i++)
            {
                hideUIElements[i].SetActive(true);
            }
        }
    }

    void AgeMatch()
    {
        if (!hasAddedCards)
        {
            if (tutorialStage == 2)
            {
                SetUp(1);
                enclosurePreview.SetActive(true);
                scoreText.enabled = true;
            }
            
        }
        if (tutorialStage == 7)
        {
            EnableCards();

            if (dropPosition.childCount > 0)
            {
                NextStage(1,8);            
            }
        }
        if (tutorialStage == 8)
        {
            ClearCards(1);
        }
    }

    void DistanceMatch()
    {
        if (!hasAddedCards)
        {
            if (tutorialStage == 11)
            {
                SetUp(2);
                bestPosition = choicePositions[0].transform.gameObject;
            }
            
        }
        if (tutorialStage == 13)
        {
            EnableCards();

            if (dropPosition.childCount > 0)
            {
                if (bestPosition.transform.childCount <= 0)
                {
                    NextStage(2, 14);
                }
                else
                {
                    TryAgain();
                }
            }
        }
        if (tutorialStage == 14)
        {
            ClearCards(2);
        }
    }

    void HealthMatch()
    {
        if (!hasAddedCards)
        {
            if (tutorialStage == 15)
            {
                SetUp(2);
                bestPosition = choicePositions[1].transform.gameObject;
            }
            
        }
        if (tutorialStage == 16)
        {
            EnableCards();

            if (dropPosition.childCount > 0)
            {
                if (bestPosition.transform.childCount <= 0)
                {
                    NextStage(3, 17);
                }
                else
                {
                    TryAgain();
                }
            }
        }
        if (tutorialStage == 17)
        {
            ClearCards(2);
        }
    }

    void ParentsMatch()
    {
        if (!hasAddedCards)
        {
            if (tutorialStage == 18)
            {
                SetUp(3);
                bestPosition = choicePositions[1].transform.gameObject;
            }
            
        }
        if (tutorialStage == 19)
        {
            EnableCards();

            if (dropPosition.childCount > 0)
            {
                if (bestPosition.transform.childCount <= 0)
                {
                    NextStage(4, 20);
                }
                else
                {
                    TryAgain();
                }
            }
        }
        if (tutorialStage == 20)
        {
            ClearCards(3);
        }
    }

    void FinalMatch()
    {
        if (!hasAddedCards)
        {
            if (tutorialStage == 22)
            {
                SetUp(3);
                bestPosition = choicePositions[0].transform.gameObject;
                tutorialStage++;
            }
        }
        if (tutorialStage == 23)
        {
            EnableCards();
            dialogueUp = false;

            if (dropPosition.childCount > 0)
            {
                if (bestPosition.transform.childCount <= 0)
                {
                    NextStage(5, 24);
                }
                else
                {
                    NextStage(4, 25);
                }
            }
        }
        if (tutorialStage >= 24)
        {
            ClearCards(3);
            dialogueUp = true;
        }
    }

    void TutorialStages()
    {
        if (dialogueUp)
        {
            dialgoueUIElements.SetActive(true);
        }
        else
        {
            dialgoueUIElements.SetActive(false);
        }

        if (tutorialStage < dialogueStrings.Count)
        {
            if (!tryAgain)
            {
                dialogueText.text = dialogueStrings[tutorialStage];
            }
        }
        else
        {
            manager.state = MatchState.hub;
        }

        if (tutorialStage == 1)
        {
            basicPlot.SetActive(true);
            canContinue = false;
        }
        else
        {
            basicPlot.SetActive(false);
        }

        if (tutorialStage == 9 || tutorialStage == 14 || tutorialStage == 17 || tutorialStage == 21)
        {
            hasAddedCards = false;
        }
        if(tutorialStage == 9 || tutorialStage == 15 || tutorialStage == 18 || tutorialStage == 22)
        {
            hasClearedCards = false;
        }

        if (tutorialStage >= 2 && tutorialStage <= 8)
        {
            AgeMatch();
        }     
        if (tutorialStage >= 11 && tutorialStage <= 14)
        {
            DistanceMatch();
        }
        if (tutorialStage >= 15 && tutorialStage <= 17)
        {
            HealthMatch();
        }
        if (tutorialStage >= 18 && tutorialStage <= 20)
        {
            ParentsMatch();
        }
        if (tutorialStage >= 21)
        {
            FinalMatch();
        }
    }

    void SetUp(int choiceNumber)
    {
        matchCard = Instantiate(cardPrefab, matchPosition.position, Quaternion.identity);
        CardDetails matchCardDetails = matchCard.GetComponent<CardDetails>();
        matchCard.transform.parent = matchPosition;
        matchCard.GetComponent<Draggable>().parentHome = matchCard.transform.parent;
        matchCard.transform.localPosition = new Vector2(0, 0);

        matchSetUp.AddCardDetails(matchCardDetails, 1, matchCards, "MatchCard");

        for (int i = 0; i < choiceNumber; i++) //for each choice position (AH)
        {
            choiceCard.Add(Instantiate(cardPrefab, choicePositions[i].position, Quaternion.identity)); //Adds a card prefab to the scene at each choice position (AH)
            CardDetails choiceCardDetails = choiceCard[i].GetComponent<CardDetails>();

            choiceCard[i].transform.parent = choicePositions[i];
            choiceCard[i].GetComponent<Draggable>().parentHome = choiceCard[i].transform.parent;
            choiceCard[i].transform.localPosition = new Vector2(0, 0); //Assigns the choice card's parent to the choice position, and sets its local position (AH)

            matchSetUp.AddCardDetails(choiceCardDetails, i + 1, choiceCards, "TutorialCard"); //Add the choice's card details (AH)
        }

        hasAddedCards = true;
        canContinue = true;
    }

    public void Next()
    {
        if (canContinue)
        {
            if (tutorialStage < dialogueStrings.Count)
            {
                tutorialStage++;
            }
            if (tutorialStage >= 24 && tutorialStage < 26)
            {
                tutorialStage = 26;
            }
        }
    }

    private void EnableCards()
    {
        List<GameObject> tutorialCards = new List<GameObject>();

        tutorialCards.AddRange(GameObject.FindGameObjectsWithTag("TutorialCard"));

        for (int i = 0; i < tutorialCards.Count; i++)
        {
            tutorialCards[i].transform.tag = "ChoiceCard";
        }

        canContinue = false;
    }

    private void ClearCards(int cullNumber)
    {
        if (!hasClearedCards)
        {
            Destroy(matchCard);

            for (int i = 0; i < choiceCard.Count; i++)
            {
                Destroy(choiceCard[i]);
            }
            choiceCard.Clear();

            matchCards.RemoveAt(0);
            choiceCards.RemoveRange(0, cullNumber);

            hasClearedCards = true;
            canContinue = true;
        }
    }

    private void TryAgain()
    {
        tryAgain = true;

        for (int i = 0; i < choicePositions.Count; i++)
        {
            if (choicePositions[i].transform.childCount <= 0)
            {
                if (dropPosition.childCount >= 1)
                {
                    dropPosition.GetChild(0).transform.parent = choicePositions[i];
                }

                if (choicePositions[i].childCount >= 1)
                {
                    choicePositions[i].GetChild(0).localPosition = new Vector2(0, 0);
                }
            }
        }

        dialogueText.text = "Try Again!";
    }

    private void NextStage(int enclosureStageNumber, int tutorialStageNumber)
    {
        tryAgain = false;
        score += 100;
        enclosureUpgrade.enclosureStage = enclosureStageNumber;
        tutorialStage = tutorialStageNumber;
    }
}
