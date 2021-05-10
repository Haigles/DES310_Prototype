using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering.Universal;
using TMPro;

public class Tutorial : MonoBehaviour
{
    private GameManager manager = null;
    private MatchSetUp matchSetUp = null;
    private Light2D globalLight = null;
    private CameraHandler cameraHandler = null;

    private bool isLightOn = true;
    private int[] darkStages = { 1, 3, 4, 5, 6, 7, 9, 10, 12, 15, 18};

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
    GameObject pointPrefab;

    [SerializeField]
    GameObject grabbedPrefab;

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
    GameObject scoreBG;

    [SerializeField]
    TMP_Text dialogueText;

    [SerializeField]
    [TextArea]
    List<string> dialogueStrings = new List<string>();

    private bool tryAgain = false;
    private bool hasAddedPoints = false;
    private bool hasClearedPoints = false;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        matchSetUp = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MatchSetUp>();
        globalLight = GameObject.FindGameObjectWithTag("GlobalLight").GetComponent<Light2D>();
        cameraHandler = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CameraHandler>();
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
        scoreBG.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.state == MatchState.tutorial)
        {
            cameraHandler.ChangeCameraPosMatch();
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

        if (!isLightOn)
        {
            globalLight.intensity = 0.35f;
        }
        else
        {
            globalLight.intensity = 1f;
        }

        if (System.Array.IndexOf(darkStages, tutorialStage) != -1)
        {
            isLightOn = false;
        }
        else
        {
            isLightOn = true;
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
                scoreBG.gameObject.SetActive(true);
            }
            
        }
        if (tutorialStage == 3)
        {
            Point(pointPrefab, matchPosition.transform.gameObject, new Vector3(110, 0, 0), 90, "Card_Name", false);
        }
        if (tutorialStage == 4)
        {
            ClearPoints();
            Point(pointPrefab, choicePositions[0].transform.gameObject, new Vector3(110, 0, 0), 90, "Card_Name", false);
        }
        if (tutorialStage == 5)
        {
            ClearPoints();
            Point(pointPrefab, matchPosition.transform.gameObject, new Vector3(110, 0, 0), 90, "Card_Age", true);
            Point(pointPrefab, choicePositions[0].transform.gameObject, new Vector3(110, 0, 0), 90, "Card_Age", false);
        }
        if (tutorialStage == 7)
        {
            EnableCards();
            ClearPoints();
            Point(grabbedPrefab, choicePositions[0].transform.gameObject, Vector3.zero, 0, "Card_Health", false);
            MoveImage(choicePositions[0].transform.position, GameObject.FindGameObjectWithTag("Point").transform, dropPosition.transform, 5f);

            if (dropPosition.childCount > 0)
            {             
                NextStage(1,8);       
            }
        }
        if (tutorialStage == 8)
        {
            ClearCards(1);
            ClearPoints();
        }
        if (tutorialStage == 9)
        {
            Point(pointPrefab, scoreText.transform.gameObject, new Vector3(-110, 75, 0), -90, "", false);
        }
        if (tutorialStage == 10)
        {
            ClearPoints();
            Point(pointPrefab, enclosurePreview.transform.gameObject, new Vector3(-80, -80, 0), -90, "", false);
        }
    }

    void DistanceMatch()
    {
        if (!hasAddedCards)
        {
            if (tutorialStage == 11)
            {
                ClearPoints();
                SetUp(2);
                bestPosition = choicePositions[0].transform.gameObject;
            }
            
        }
        if (tutorialStage == 12)
        {
            Point(pointPrefab, choicePositions[0].transform.gameObject, new Vector3(0, -60, 0), 0, "Card_Location_Info", true);
            Point(pointPrefab, choicePositions[1].transform.gameObject, new Vector3(0, -60, 0), 0, "Card_Location_Info", false);
        }
        if (tutorialStage == 13)
        {
            ClearPoints();
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
                Point(pointPrefab, choicePositions[0].transform.gameObject, new Vector3(0, 115, 0), 180, "Card_Health_Info", true);
                Point(pointPrefab, choicePositions[1].transform.gameObject, new Vector3(0, 115, 0), 180, "Card_Health_Info", false);
            }
            
        }
        if (tutorialStage == 16)
        {
            ClearPoints();
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

                Point(pointPrefab, choicePositions[0].transform.gameObject, new Vector3(0, 100, 0), 180, "Card_Parents_Male_Info", true);
                Point(pointPrefab, choicePositions[1].transform.gameObject, new Vector3(0, 100, 0), 180, "Card_Parents_Male_Info", true);
                Point(pointPrefab, choicePositions[2].transform.gameObject, new Vector3(0, 100, 0), 180, "Card_Parents_Male_Info", false);
            }
            
        }
        if (tutorialStage == 19)
        {
            ClearPoints();
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
            cameraHandler.ChangeCameraPosSelect();
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

        if (tutorialStage >= 2 && tutorialStage <= 10)
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

            hasAddedPoints = false;
            hasClearedPoints = false;
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
        hasClearedPoints = false;
        hasAddedPoints = false;
        tryAgain = false;
        score += 100;
        enclosureUpgrade.enclosureStage = enclosureStageNumber;
        tutorialStage = tutorialStageNumber;
    }

    private void Point(GameObject prefab, GameObject originalParent, Vector3 offset, float rotation, string textField, bool extra)
    {
        if (!hasAddedPoints)
        {
            GameObject point = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            GameObject intermediateParent;
            GameObject finalParent;

            point.transform.parent = originalParent.transform.parent;

            if (originalParent.transform.childCount > 0)
            {
                for (int i = 0; i < originalParent.transform.childCount; i++)
                {
                    if (originalParent.transform.GetChild(i).tag == "MatchCard" || originalParent.transform.GetChild(i).tag == "TutorialCard" || originalParent.transform.GetChild(i).tag == "ChoiceCard")
                    {
                        intermediateParent = originalParent.transform.GetChild(i).gameObject;

                        if (intermediateParent.transform.childCount > 0)
                        {
                            foreach (Transform eachChild in intermediateParent.transform)
                            {
                                if (eachChild.name == textField)
                                {
                                    finalParent = eachChild.gameObject;
                                    point.transform.parent = finalParent.transform;
                                }
                            }
                        }
                    }
                }
            }

            point.transform.localPosition = offset;
            point.transform.rotation = Quaternion.Euler(0f, 0f, rotation);

            if (!extra)
            {
                hasAddedPoints = true;
            }
        }
    }

    private void ClearPoints()
    {
        if (!hasClearedPoints)
        {
            for (int i = 0; i < GameObject.FindGameObjectsWithTag("Point").Length; i++)
            {
                Destroy(GameObject.FindGameObjectsWithTag("Point")[i]);
            }

            hasClearedPoints = true;
        }
    }

    private void MoveImage(Vector3 originalPosition, Transform obj, Transform target, float speed)
    {
        obj.parent = GameObject.FindGameObjectWithTag("Canvas").transform;

        obj.transform.position = Vector3.MoveTowards(obj.position, target.position, speed * Time.deltaTime);

        if (Vector3.Distance(obj.position, target.position) <= 0.1f)
        {
            obj.position = originalPosition;
        }
    }
}
