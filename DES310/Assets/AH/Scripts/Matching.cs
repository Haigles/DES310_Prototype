using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Matching : MonoBehaviour
{
    [Header("Timer Variables")]
    public bool activateTimer = false;
    
    public float assignTimer = 0;
    [HideInInspector]
    public float timer = 0;
    private bool timerIsRunning;
    
    [Header("UI Elements")]
    [Space(10)]
    public GameObject timerUI;
    public GameObject keeperGoodMatch;
    public GameObject keeperBadMatch;
    public GameObject keeperOkayMatch;
    public Text textTimer;
    public Text scoreText;
    public Text stageIndicator;

    [Header("HS Testing")]
    public bool goodMatch;
    public bool badMatch;
    public bool okayMatch;
    public List<GameObject> StageIcon = new List<GameObject>();

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
    List<int> stageScoreThreshold;
    public int currentStageIcon = 0;
    
    private MatchSetUp matchSetUp;
    private GameManager manager;
    private CheckClicks canvas;
    private Recap recap;

    // Start is called before the first frame update
    void Start()
    {
        currentStageIcon = 1;
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        recap = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Recap>();
        matchSetUp = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MatchSetUp>();
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CheckClicks>();

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

            manager.state = MatchState.matching; //Changes game state to 'Matching' (AH)
        }
    }

    static int SortByValue(GameObject value1, GameObject value2)
    {
        return value1.GetComponent<CardDetails>().cardTotalValue.CompareTo(value2.GetComponent<CardDetails>().cardTotalValue);
        //Sorts list based on the cards total value, with the smallest number appearing first (AH)
    }

    private void CheckMatch()
    {
        scoreText.enabled = true; //enable score text UI (AH)

        for (int i = 0; i < canvas.clickResults.Count; i++) //Get all results from 'CheckClick' (AH)
        {
            if (canvas.clickResults[i].gameObject.transform.tag == "ChoiceCard") //If result has tag 'ChoiceCard' (AH)
            {
                if (GameObject.ReferenceEquals(canvas.clickResults[i].gameObject, currentChoices[0])) //If choice card is first in list (AH)
                {
                    Debug.Log("Good Match");
                    goodMatch = true;
                    badMatch = false;
                    okayMatch = false;
                    AddCardToRecap(currentMatchCard, currentChoices[0]); //Add match card and chosen choice card to recap (AH)
                    CalculateScore(bestMatchScore); //Give best score (AH)
                    
                }
                else if (GameObject.ReferenceEquals(canvas.clickResults[i].gameObject, currentChoices[1])) //If choice card is secong in list (AH)
                {
                    Debug.Log("Middle Match");
                    goodMatch = false;
                    badMatch = false;
                    okayMatch = true;
                    AddCardToRecap(currentMatchCard, currentChoices[1]); //Add match card and chosen choice card to recap (AH)
                    CalculateScore(middleMatchScore); //Give middle score (AH)
                    
                }
                else if (GameObject.ReferenceEquals(canvas.clickResults[i].gameObject, currentChoices[2])) //If choice card is third in list (AH)
                {
                    Debug.Log("Bad Match");
                    goodMatch = false;
                    badMatch = true;
                    okayMatch = false;
                    AddCardToRecap(currentMatchCard, currentChoices[2]); //Add match card and chosen choice card to recap (AH)
                    CalculateScore(worstMatchScore); //Give worst score (AH)
                    
                }

                DrawNewCards(); //Draw new cards (AH)

            }
        }
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
        int stageCounter = 1; //start stage counter at 1 (AH)

        score += scoreUpdate; //increase score by 'scoreUpdate' (AH)
        recap.compatability.Add(scoreUpdate); //adds 'scoreUpdate' value to the recap compatability list (AH)

        scoreText.text = "Score: " + score; //Updates score UI text (AH)
        recap.recapScore.text = "" + score; //Updates the recap score UI text (AH)

        for (int i = 0; i < stageScoreThreshold.Count; i++)
        {
            
            if (score >= stageScoreThreshold[i]) //if score is above a certain score thresthold (AH)
            {
                StageIcon[currentStageIcon].SetActive(false);
                currentStageIcon++;
                if (currentStageIcon >= StageIcon.Count)
                    currentStageIcon = 4;
                StageIcon[currentStageIcon].SetActive(true);
                stageCounter++; //increase stage counter by 1 (AH)
                stageIndicator.text = "Stage " + stageCounter; //Updates stage inicator UI text (AH)
                
                recap.stageIndicator.text = "Stage " + stageCounter; //Updates recap stage inicator UI text (AH)
                
            }
        }
    }

    private void AddCardToRecap(GameObject matchCard, GameObject choiceCard)
    {
        GameObject duplicateMatchCard = Instantiate(matchCard, recap.matchPosition.position, Quaternion.identity); //adds a duplicate match card for the recap (AH)
        GameObject duplicateChoiceCard = Instantiate(choiceCard, recap.choicePosition.position, Quaternion.identity); //adds a duplicate choice card that was picked for the recap (AH)

        duplicateMatchCard.transform.parent = recap.matchPosition; //assigns the duplicate match card's parent to the recap position (AH)
        duplicateChoiceCard.transform.parent = recap.choicePosition; //assigns the duplicate choice card's parent to the recap position (AH)

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

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        textTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds); //Formats timer text into minutes and seconds (AH)
    }
}


