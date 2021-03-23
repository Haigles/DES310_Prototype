using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Matching : MonoBehaviour
{
    public Text textTimer;
    public float timer;
    private bool timerIsRunning;
    public Text scoreText;
    public Text stageIndicator;
    public MatchSetUp matchSetUp;
    public int parentPenalty;
    private GameManager manager;
    private CheckClicks canvas;
    public GameObject currentMatchCard;
    public List<GameObject> currentChoices;
    private int score;

    [SerializeField]
    int bestMatchScore, middleMatchScore, worstMatchScore;

    [SerializeField]
    List<int> stageScoreThreshold;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CheckClicks>();
        scoreText.enabled = false;
        textTimer.enabled = false;
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
                    CalculateScore(bestMatchScore);
                }
                else if (GameObject.ReferenceEquals(canvas.clickResults[i].gameObject, currentChoices[1]))
                {
                    CalculateScore(middleMatchScore);
                }
                else if (GameObject.ReferenceEquals(canvas.clickResults[i].gameObject, currentChoices[2]))
                {
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
        else
        {
            scoreText.enabled = false;
            manager.state = MatchState.recap;
        }
    }

    private void CalculateScore(int scoreUpdate)
    {
        int stageCounter = 1;

        score += scoreUpdate;
        scoreText.text = "Score: " + score;

        for (int i = 0; i < stageScoreThreshold.Count; i++)
        {
            if (score >= stageScoreThreshold[i])
            {
                stageCounter++;
                stageIndicator.text = "Stage " + stageCounter;
            }
        }
    }

    private void Timer()
    {
        DisplayTimer(timer);

        if (timerIsRunning)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                timer = 0;
                manager.state = MatchState.recap;
                timerIsRunning = false;
            }
        }
    }

    private void DisplayTimer(float timeToDisplay)
    {
        textTimer.enabled = true;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        textTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}


