using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Matching : MonoBehaviour
{
    [Header("Timer Variables")]
    public bool activateTimer = false;
    public GameObject timerUI;
    public Text textTimer;
    public float assignTimer = 0;
    [HideInInspector]
    public float timer = 0;
    private bool timerIsRunning;
    [Space(25)]

    public Text scoreText;
    public Text stageIndicator;
    private MatchSetUp matchSetUp;
    public int parentPenalty;
    private GameManager manager;
    private CheckClicks canvas;
    private Recap recap;
    public GameObject currentMatchCard;
    public List<GameObject> currentChoices;
    public int score;

    [SerializeField]
    int bestMatchScore, middleMatchScore, worstMatchScore;

    [SerializeField]
    List<int> stageScoreThreshold;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        recap = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Recap>();
        matchSetUp = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MatchSetUp>();

        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CheckClicks>();

        timer = assignTimer;

        scoreText.enabled = false;
        timerIsRunning = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.state == MatchState.calculate)
        {
            currentMatchCard = matchSetUp.matchCard;
            currentChoices = matchSetUp.choices;

            CalculateBestMatch(currentChoices);
            currentChoices.Sort(SortByValue);
        }

        if (manager.state == MatchState.matching)
        {
            CheckMatch();

            timerIsRunning = true;
            Timer();
        }
        else
        {
            timerUI.SetActive(false);
        }
    }

    private void CalculateBestMatch(List<GameObject> choices)
    {
        CardDetails matchDetails = currentMatchCard.GetComponent<CardDetails>();

        for (int i = 0; i < choices.Count; i++)
        {
            int totalValue = 0;

            CardDetails choiceDetails = choices[i].GetComponent<CardDetails>();

            int totalAgeValue = Mathf.Abs(matchDetails.valueAge - choiceDetails.valueAge);
            int totalLocationValue = choiceDetails.valueDistance;
            int totalHealthValue = matchDetails.valueHealth + choiceDetails.valueHealth;
            int totalParentValue = 0;

            if(matchDetails.maleParent == choiceDetails.maleParent)
            {
                totalParentValue += parentPenalty;
            }

            if (matchDetails.femaleParent == choiceDetails.femaleParent)
            {
                totalParentValue += parentPenalty;
            }

            totalValue = totalAgeValue + totalHealthValue + totalLocationValue + totalParentValue;

            choiceDetails.cardTotalValue = totalValue;

            manager.state = MatchState.matching;
        }
    }

    static int SortByValue(GameObject value1, GameObject value2)
    {
        return value1.GetComponent<CardDetails>().cardTotalValue.CompareTo(value2.GetComponent<CardDetails>().cardTotalValue);
    }

    private void CheckMatch()
    {
        scoreText.enabled = true;

        for (int i = 0; i < canvas.clickResults.Count; i++)
        {
            if (canvas.clickResults[i].gameObject.transform.tag == "ChoiceCard")
            {
                if (GameObject.ReferenceEquals(canvas.clickResults[i].gameObject, currentChoices[0]))
                {
                    AddCardToRecap(currentMatchCard, currentChoices[0]);
                    CalculateScore(bestMatchScore); 
                }
                else if (GameObject.ReferenceEquals(canvas.clickResults[i].gameObject, currentChoices[1]))
                {
                    AddCardToRecap(currentMatchCard, currentChoices[1]);
                    CalculateScore(middleMatchScore);
                }
                else if (GameObject.ReferenceEquals(canvas.clickResults[i].gameObject, currentChoices[2]))
                {
                    AddCardToRecap(currentMatchCard, currentChoices[2]);
                    CalculateScore(worstMatchScore);
                }

                DrawNewCards();
            }
        }
    }

    private void DrawNewCards()
    {
        for (int i = 0; i < currentChoices.Count; i++)
        {
            Destroy(currentChoices[i]);
        }

        Destroy(currentMatchCard);
        canvas.clickResults.Clear();

        if (matchSetUp.choiceCards.Count > 0 || matchSetUp.matchCards.Count > 0)
        {
            matchSetUp.choiceCards.RemoveRange(0, 3);
            matchSetUp.choices.Clear();
            matchSetUp.Shuffle(matchSetUp.choiceCards);

            matchSetUp.matchCards.RemoveAt(0);
            matchSetUp.Shuffle(matchSetUp.matchCards);

            manager.state = MatchState.setUp;
        }
    }

    private void CalculateScore(int scoreUpdate)
    {
        int stageCounter = 1;

        score += scoreUpdate;
        recap.compatability.Add(scoreUpdate);

        scoreText.text = "Score: " + score;
        recap.recapScore.text = "" + score;

        for (int i = 0; i < stageScoreThreshold.Count; i++)
        {
            if (score >= stageScoreThreshold[i])
            {
                stageCounter++;
                stageIndicator.text = "Stage " + stageCounter;
                recap.stageIndicator.text = "Stage " + stageCounter;
            }
        }
    }

    private void AddCardToRecap(GameObject matchCard, GameObject choiceCard)
    {
        GameObject duplicateMatchCard = Instantiate(matchCard, recap.matchPosition.position, Quaternion.identity);
        GameObject duplicateChoiceCard = Instantiate(choiceCard, recap.choicePosition.position, Quaternion.identity);

        duplicateMatchCard.transform.parent = recap.matchPosition;
        duplicateChoiceCard.transform.parent = recap.choicePosition;

        recap.matchCards.Add(duplicateMatchCard);
        recap.choiceCards.Add(duplicateChoiceCard);
    }

    private void Timer()
    {
        DisplayTimer(timer);

        if (timerIsRunning)
        {
            if (timer > 0)
            {
                if (activateTimer)
                {
                    timer -= Time.deltaTime;
                }
            }
            else
            {
                timer = 0;
                //Debug.Log("RECAP FROM TIMER");
                manager.state = MatchState.recap;
                timerIsRunning = false;
            }
        }
    }

    private void DisplayTimer(float timeToDisplay)
    {
        timerUI.SetActive(true);

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        textTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}


