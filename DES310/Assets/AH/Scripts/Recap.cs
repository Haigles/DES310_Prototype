using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Recap : MonoBehaviour
{
    public GameObject recapScreen;
    public GameObject backButton;
    public GameObject nextButton;
    public TMP_Text textPercentNumber;
    public Text recapScore;
    public Text stageIndicator;
    public Transform matchPosition;
    public Transform choicePosition;
    private int checkMatchInt = 0;

    private GameManager manager;
    private GameObject mainCamera;
    private MatchSetUp matchSetUp;
    private Matching matching;
    public AnimalSelection animalSelection;

    [SerializeField]
    List<GameObject> hideUI;

    public List<GameObject> matchCards = new List<GameObject>();
    public List<GameObject> choiceCards = new List<GameObject>();
    public List<int> compatability = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        recapScreen.SetActive(false);
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        matchSetUp = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MatchSetUp>();
        matching = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Matching>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.state == MatchState.recap)
        {
            recapScreen.SetActive(true);
            HideUIElements();
            ShowMatches();
        }
        else
        {
            recapScreen.SetActive(false);
        }

        if (manager.state == MatchState.reset)
        {
            ResetMatching();
        }
    }

    private void HideUIElements()
    {
        for (int i = 0; i < hideUI.Count; i++)
        {
            hideUI[i].SetActive(false);
        }

        if (checkMatchInt == 0)
        {
            backButton.SetActive(false);
        }
        else
        {
            backButton.SetActive(true);
        }

        if (checkMatchInt == matchCards.Count - 1)
        {
            nextButton.SetActive(false);
        }
        else
        {
            nextButton.SetActive(true);
        }
    }

    private void ShowMatches()
    {
        for (int i = 0; i < matchCards.Count; i++)
        {
            if (i == checkMatchInt)
            {
                matchCards[checkMatchInt].SetActive(true);
                choiceCards[checkMatchInt].SetActive(true);
            }
            else
            {
                matchCards[i].SetActive(false);
                choiceCards[i].SetActive(false);
            }

            textPercentNumber.text = "" + compatability[checkMatchInt];
        }
    }

    private void ResetMatching()
    {
        mainCamera.transform.position = new Vector3(0, 0, -10);

        if (matchCards != null)
        {
            for (int i = 0; i < matchCards.Count; i++)
            {
                Destroy(choiceCards[i].gameObject);
                Destroy(matchCards[i].gameObject);
            }

            compatability.Clear();
            choiceCards.Clear();
            matchCards.Clear();
        }

        animalSelection.selectedEnclosure.animalIndicator.text = matchSetUp.animalIndicator.text;
        animalSelection.selectedEnclosure.stageIndicator.text = stageIndicator.text;
        checkMatchInt = 0;
        matchSetUp.addedCards = false;
        matching.timer = matching.assignTimer;
        matching.score = 0;
        matching.scoreText.text = "Score: " + matching.score;
        matching.stageIndicator.text = "Stage 1";
        manager.state = MatchState.hub;
    }

    public void NextButton()
    {
        if (checkMatchInt < matchCards.Count - 1)
        {
            checkMatchInt++;
        }
    }

    public void BackButton()
    {
        if (checkMatchInt > 0)
        {
            checkMatchInt--;
        }
    }

    public void HomeButton()
    {
        manager.state = MatchState.reset;
    }
}
