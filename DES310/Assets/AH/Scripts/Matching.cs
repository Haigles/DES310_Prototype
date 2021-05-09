using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Matching : MonoBehaviour
{
    [Header("Timer Variables")]
    public bool activateTimer = false;
    public float assignTimer = 0f;
    public float increaseTimer = 0f;
    [HideInInspector]
    public float timer = 0;
    private bool timerIsRunning;
    
    [Header("UI Elements")]
    [Space(10)]
    public GameObject timerUI;
    public GameObject increasePrefab;
    public GameObject studbookButton;
    public GameObject actualStudbook;
    public GameObject keeperGoodMatch;
    public GameObject keeperBadMatch;
    public GameObject keeperOkayMatch;
    public TMP_Text textTimer;
    public TMP_Text scoreText;

    [Header("HS Testing")]
    public bool goodMatch;
    public bool badMatch;
    public bool okayMatch;
    public AnimalSelection animalSelection;
    //public List<Sprite> stageEnclosureSprites = new List<Sprite>();
    //public List<Sprite> penguinStageEnclosureSprites = new List<Sprite>();
    public GameObject stageEnclosure;

    [Header("Current Matches")]
    [Space(10)]
    public int parentPenalty;  
    public GameObject currentMatchCard;
    public List<GameObject> currentChoices;
    
    [Header("Score Variables")]
    [Space(10)]
    public int score = 0;
    [SerializeField]
    int bestMatchScore = 0, middleMatchScore = 0, worstMatchScore = 0;
    [SerializeField]
    List<int> combos = new List<int>();
    public int comboStage = 0;
    private int highestCombo = 0;
    [SerializeField]
    List<int> stageScoreThreshold;
    public int currentStageIcon = 0;
    
    private MatchSetUp matchSetUp;
    private GameManager manager;
    private CheckClicks canvas;
    private Recap recap;
    private GameObject dropPosition;

    // Start is called before the first frame update
    void Start()
    {
        currentStageIcon = 1;
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        recap = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Recap>();
        matchSetUp = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MatchSetUp>();
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CheckClicks>();
        dropPosition = GameObject.FindGameObjectWithTag("Drop");

        timer = assignTimer; //Sets timer to assigned variable (AH)

        scoreText.enabled = false;
        timerIsRunning = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.state == MatchState.calculate) //When the game manager is on the 'Calculate' state (AH)
        {
            currentMatchCard = matchSetUp.matchCard; //Get match card from 'MatchSetUp Script' (AH)
            currentChoices = matchSetUp.choices; //Get choice cards from 'MatchSetUp Script' (AH)

            CalculateBestMatch(currentChoices); //Calculates best matches from the current choice card pool (AH)
            currentChoices.Sort(SortByValue); //Sorts choice cards based on their 'totalCardValue' (AH)
        }

        if (manager.state == MatchState.matching) //When the game manager is on the 'Matching' state (AH)
        {
            CheckMatch();
            if(goodMatch == true)
            {
                
                keeperGoodMatch.SetActive(true);
                keeperBadMatch.SetActive(false);
                keeperOkayMatch.SetActive(false);
            }
            else if(okayMatch == true)
            {
                
                keeperGoodMatch.SetActive(false);
                keeperBadMatch.SetActive(false);
                keeperOkayMatch.SetActive(true);
            }
            else if(badMatch == true)
            {
                keeperGoodMatch.SetActive(false);
                keeperBadMatch.SetActive(true);
                keeperOkayMatch.SetActive(false);              
            }

            timerIsRunning = true; //Starts Timer (AH)
            Timer();
        }
        else //When the game manager is NOT on the 'Matching' state (AH)
        {
            timerUI.SetActive(false); //Hides Timer UI (AH)
            studbookButton.SetActive(false);
            actualStudbook.SetActive(false);
            keeperGoodMatch.SetActive(false);
            keeperBadMatch.SetActive(false);
            keeperOkayMatch.SetActive(false);
        }
    }

    private void CalculateBestMatch(List<GameObject> choices)
    {
        CardDetails matchDetails = currentMatchCard.GetComponent<CardDetails>(); //Gets current match's 'Card Details' (AH)

        for (int i = 0; i < choices.Count; i++) //For every choice card (AH)
        {
            int totalValue = 0;

            CardDetails choiceDetails = choices[i].GetComponent<CardDetails>(); //Get choice card details (AH)

            int totalAgeValue = Mathf.Abs(matchDetails.valueAge - choiceDetails.valueAge); //Gets the difference in ages between the match and choice card (AH)
            int totalLocationValue = choiceDetails.valueDistance; //Gets assigned distance value from choice card (AH)
            int totalHealthValue = matchDetails.valueHealth + choiceDetails.valueHealth; //Adds health values from both the match and choice cards (AH)
            int totalParentValue = 0;

            if(matchDetails.maleParent == choiceDetails.maleParent)
            {
                totalParentValue += parentPenalty; //If both cards share a male parent, add parent penalty (AH)
            }

            if (matchDetails.femaleParent == choiceDetails.femaleParent)
            {
                totalParentValue += parentPenalty; //If both cards share a female parent, add parent penalty (AH)
            }

            totalValue = totalAgeValue + totalHealthValue + totalLocationValue + totalParentValue; //add all values together (AH)

            choiceDetails.cardTotalValue = totalValue; //Assigns the cards total value based on match (AH)

            CalculateDifference(choiceDetails, totalAgeValue, totalLocationValue, totalHealthValue, totalParentValue);

            manager.state = MatchState.matching; //Changes game state to 'Matching' (AH)
        }
    }

    private void CalculateDifference(CardDetails cardDetails, int age, int distance, int health, int parents)
    {
        cardDetails.values.Add(age);
        cardDetails.values.Add(distance);
        cardDetails.values.Add(health);
        cardDetails.values.Add(parents);

        cardDetails.values.Sort(SortDifferences);

        if (cardDetails.values[3] == age)
        {
            cardDetails.highAge = true;
            cardDetails.highLocation = false;
            cardDetails.highHealth = false;
            cardDetails.highParents = false;
        }
        else if (cardDetails.values[3] == distance)
        {
            cardDetails.highAge = false;
            cardDetails.highLocation = true;
            cardDetails.highHealth = false;
            cardDetails.highParents = false;
        }
        else if (cardDetails.values[3] == health)
        {
            cardDetails.highAge = false;
            cardDetails.highLocation = false;
            cardDetails.highHealth = true;
            cardDetails.highParents = false;
        }
        else if (cardDetails.values[3] == parents)
        {
            cardDetails.highAge = false;
            cardDetails.highLocation = false;
            cardDetails.highHealth = false;
            cardDetails.highParents = true;
        }
    }

    static int SortByValue(GameObject value1, GameObject value2)
    {
        return value1.GetComponent<CardDetails>().cardTotalValue.CompareTo(value2.GetComponent<CardDetails>().cardTotalValue);
        //Sorts list based on the cards total value, with the smallest number appearing first (AH)
    }

    static int SortDifferences(int value1, int value2)
    {
        return value1.CompareTo(value2);
        //Sorts list based on the cards total value, with the smallest number appearing first (AH)
    }

    private void CheckMatch()
    {
        scoreText.enabled = true; //enable score text UI (AH)

        if (dropPosition.transform.childCount > 0)
        {
            Transform[] child = dropPosition.transform.GetComponentsInChildren<Transform>();

            for (int i = 0; i < child.Length; i++)
            {
                if (child[i].gameObject == currentChoices[0])
                {
                    //Debug.Log("Good Match");
                    goodMatch = true;
                    badMatch = false;
                    okayMatch = false;
                    currentChoices[0].GetComponent<CardDetails>().matchValue = 0;
                    AddCardToRecap(currentMatchCard, currentChoices[0]); //Add match card and chosen choice card to recap (AH)
                    comboStage++;
                    CheckCombo(comboStage);
                    timer += increaseTimer;
                    InstantiateIncrease("+" + increaseTimer, timerUI);
                    InstantiateIncrease("Combo x" + comboStage, scoreText.transform.gameObject);
                    CalculateScore(bestMatchScore + combos[comboStage]); //Give best score (AH)
                }
                else if (child[i].gameObject == currentChoices[1])
                {
                    //Debug.Log("Middle Match");
                    goodMatch = false;
                    badMatch = false;
                    okayMatch = true;
                    currentChoices[1].GetComponent<CardDetails>().matchValue = 1;
                    AddCardToRecap(currentMatchCard, currentChoices[1]); //Add match card and chosen choice card to recap (AH)
                    comboStage = 0;
                    CalculateScore(middleMatchScore + combos[comboStage]); //Give middle score (AH)
                }
                else if (child[i].gameObject == currentChoices[2])
                {
                    //Debug.Log("Bad Match");
                    goodMatch = false;
                    badMatch = true;
                    okayMatch = false;
                    currentChoices[2].GetComponent<CardDetails>().matchValue = 2;
                    AddCardToRecap(currentMatchCard, currentChoices[2]); //Add match card and chosen choice card to recap (AH)
                    comboStage = 0;
                    CalculateScore(worstMatchScore + combos[comboStage]); //Give worst score (AH)
                }
            }

            DrawNewCards(); //Draw new cards (AH)
        }

        //for (int i = 0; i < canvas.clickResults.Count; i++) //Get all results from 'CheckClick' (AH)
       // {

            //if (canvas.clickResults[i].gameObject.transform.tag == "ChoiceCard") //If result has tag 'ChoiceCard' (AH)
            //{
                //if (GameObject.ReferenceEquals(canvas.clickResults[i].gameObject, currentChoices[0])) //If choice card is first in list (AH)
                //{
                //    Debug.Log("Good Match");
                //    goodMatch = true;
                //    badMatch = false;
                //    okayMatch = false;
                //    AddCardToRecap(currentMatchCard, currentChoices[0]); //Add match card and chosen choice card to recap (AH)
                //    CalculateScore(bestMatchScore); //Give best score (AH)

                //}
                //else if (GameObject.ReferenceEquals(canvas.clickResults[i].gameObject, currentChoices[1])) //If choice card is secong in list (AH)
                //{
                //    Debug.Log("Middle Match");
                //    goodMatch = false;
                //    badMatch = false;
                //    okayMatch = true;
                //    AddCardToRecap(currentMatchCard, currentChoices[1]); //Add match card and chosen choice card to recap (AH)
                //    CalculateScore(middleMatchScore); //Give middle score (AH)

                //}
                //else if (GameObject.ReferenceEquals(canvas.clickResults[i].gameObject, currentChoices[2])) //If choice card is third in list (AH)
                //{
                //    Debug.Log("Bad Match");
                //    goodMatch = false;
                //    badMatch = true;
                //    okayMatch = false;
                //    AddCardToRecap(currentMatchCard, currentChoices[2]); //Add match card and chosen choice card to recap (AH)
                //    CalculateScore(worstMatchScore); //Give worst score (AH)

                //}

                //canvas.clickResults[i].GetComponent<Draggable>().dragging = true;
                //DrawNewCards(); //Draw new cards (AH)

            //}
        //}
    }

    private void DrawNewCards()
    {
        for (int i = 0; i < currentChoices.Count; i++)
        {
            Destroy(currentChoices[i]); //Destroy all cureent choice cards (AH)
        }

        Destroy(currentMatchCard); //Destroy current match card (AH)
        canvas.clickResults.Clear(); //Clear click results (AH)

        if (matchSetUp.choiceCards.Count > 0 || matchSetUp.matchCards.Count > 0)
        {
            matchSetUp.choiceCards.RemoveRange(0, 3); //Remove top 3 choice cards from card pool (AH)
            matchSetUp.choices.Clear(); //Clear choice list (AH)
            matchSetUp.Shuffle(matchSetUp.choiceCards); //Shuffle remaining choice cards (AH)

            matchSetUp.matchCards.RemoveAt(0); //Remove top match card (AH)
            matchSetUp.Shuffle(matchSetUp.matchCards); //Shuffle remaining match cards (AH)

            manager.state = MatchState.setUp; //Changes game state to 'SetUp' (AH)
        }
    }

    private void ClearAllCards()
    {
        for (int i = 0; i < currentChoices.Count; i++)
        {
            Destroy(currentChoices[i]); //Destroy all cureent choice cards (AH)

        }
        Destroy(currentMatchCard); //Destroy current match card (AH)

        canvas.clickResults.Clear(); //Clear click results (AH)
        matchSetUp.choiceCards.Clear(); //Clear choice cards from card pool (AH)
        matchSetUp.choices.Clear(); //Clear choice list (AH)
        matchSetUp.matchCards.Clear(); //Clear match cards (AH)       
    }

    private void CalculateScore(int scoreUpdate)
    {
        int stageCounter = 0; //start stage counter at 1 (AH)

        score += scoreUpdate; //increase score by 'scoreUpdate' (AH)
        scoreUpdate -= combos[comboStage];
        recap.compatability.Add(scoreUpdate); //adds 'scoreUpdate' value to the recap compatability list (AH)

        scoreText.text = "Score: " + score; //Updates score UI text (AH)
        //recap.recapScore.text = "" + score; //Updates the recap score UI text (AH)

        for (int i = 0; i < stageScoreThreshold.Count; i++)
        {
            
            if (score >= stageScoreThreshold[i]) //if score is above a certain score thresthold (AH)
            {
                stageCounter++;
                matchSetUp.enclosurePreview.enclosureStage = (stageCounter);//increase stage counter by 1 (AH)
                stageEnclosure.GetComponent<SpriteRenderer>().sprite = matchSetUp.enclosurePreview.GetComponent<SpriteRenderer>().sprite;
            }
        }
    }

    private void AddCardToRecap(GameObject matchCard, GameObject choiceCard)
    {
        float scale = 0.66f;
        GameObject duplicateMatchCard = Instantiate(matchCard, recap.matchPosition.position, Quaternion.identity); //adds a duplicate match card for the recap (AH)
        GameObject duplicateChoiceCard = Instantiate(choiceCard, recap.choicePosition.position, Quaternion.identity); //adds a duplicate choice card that was picked for the recap (AH)
        CardDetails duplicateChoiceCardDetails = duplicateChoiceCard.GetComponent<CardDetails>();
        CardDetails choiceCardDetails = choiceCard.GetComponent<CardDetails>();

        duplicateMatchCard.transform.parent = recap.matchPosition; //assigns the duplicate match card's parent to the recap position (AH)
        duplicateChoiceCard.transform.parent = recap.choicePosition; //assigns the duplicate choice card's parent to the recap position (AH)

        duplicateChoiceCard.transform.localScale = new Vector3(scale, scale, 1);
        duplicateChoiceCard.transform.tag = "Untagged";
        duplicateChoiceCardDetails.matchValue = choiceCardDetails.matchValue;
        duplicateChoiceCardDetails.highAge = choiceCardDetails.highAge;
        duplicateChoiceCardDetails.highLocation = choiceCardDetails.highLocation;
        duplicateChoiceCardDetails.highHealth = choiceCardDetails.highHealth;
        duplicateChoiceCardDetails.highParents = choiceCardDetails.highParents;

        duplicateMatchCard.transform.localScale = new Vector3(scale, scale, 1);
        duplicateMatchCard.transform.tag = "Untagged";

        recap.matchCards.Add(duplicateMatchCard); //adds the duplicate match card to the recap match list (AH)
        recap.choiceCards.Add(duplicateChoiceCard); //adds the duplicate choice card to the recap choice list (AH)
    }

    private void Timer()
    {
        DisplayTimer(timer);

        if (timerIsRunning)
        {
            if (timer > 0) //if timer is above 0 (AH)
            {
                if (activateTimer)
                {
                    timer -= Time.deltaTime; //deplete timer (AH)
                }
            }
            else
            {
                timer = 0; //keeps timer at 0 (AH)
                ClearAllCards(); //Clears and destroys all cards (AH) 
                manager.state = MatchState.recap; //Changes game state to 'Recap' (AH)
                timerIsRunning = false;
            }
        }
    }

    private void DisplayTimer(float timeToDisplay)
    {
        timerUI.SetActive(true); //Shows timer UI element (AH)
        studbookButton.SetActive(true);

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        textTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds); //Formats timer text into minutes and seconds (AH)
    }

    private void InstantiateIncrease(string text, GameObject parent)
    {
        GameObject increase = Instantiate(increasePrefab, Vector3.zero, Quaternion.identity);
        increase.transform.parent = parent.transform;
        increase.transform.localPosition = Vector3.zero;
        increase.GetComponent<Increase>().text.text = text;
    }

    private void CheckCombo(int currentCombo)
    {
        if (currentCombo >= highestCombo)
        {
            highestCombo = currentCombo;
            recap.comboStage = highestCombo;
        }
    }
}


