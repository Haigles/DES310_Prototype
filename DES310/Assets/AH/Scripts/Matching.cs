using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Matching : MonoBehaviour
{
    public RawImage matchIndicator;
    public MatchSetUp matchSetUp;
    public int parentPenalty;
    private GameManager manager;
    private CheckClicks canvas;
    public GameObject currentMatchCard;
    public List<GameObject> currentChoices;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CheckClicks>();
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
        for (int i = 0; i < canvas.clickResults.Count; i++)
        {
            if (canvas.clickResults[i].gameObject.transform.tag == "ChoiceCard")
            {
                if (GameObject.ReferenceEquals(canvas.clickResults[i].gameObject, currentChoices[0]))
                {
                    matchIndicator.color = new Color32(0, 128, 0, 255); 
                }
                else if (GameObject.ReferenceEquals(canvas.clickResults[i].gameObject, currentChoices[1]))
                {
                    matchIndicator.color = new Color32(255, 255, 0, 255);
                }
                else if (GameObject.ReferenceEquals(canvas.clickResults[i].gameObject, currentChoices[2]))
                {
                    matchIndicator.color = new Color32(255, 0, 0, 255);
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
            manager.state = MatchState.recap;
        }
    }
}


