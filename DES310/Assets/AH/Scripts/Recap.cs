using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Recap : MonoBehaviour
{
    [Header("Animal Selection")]
    public AnimalSelection animalSelection;
    public UpgradeEnclosure enclosurePreview;

    [Header("UI GameObjects")]
    [Space(10)]
    public GameObject recapScreen;
    public GameObject backButton;
    public GameObject nextButton;

    [Header("UI Text Elements")]
    [Space(10)]
    public TMP_Text textPercentNumber;
    public TMP_Text recapScore;

    [Header("Card Positions")]
    [Space(10)]
    public Transform matchPosition;
    public Transform choicePosition;
      
    private int checkMatchInt = 0;
    private GameManager manager;
    private GameObject mainCamera;
    private MatchSetUp matchSetUp;
    private Matching matching;
    

    [Header("Hide These UI Elements")]
    [Space(10)]
    [SerializeField]
    List<GameObject> hideUI;
    public GameObject background;

    [Header("Updates from Matching")]
    [Space(10)]
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
        if (manager.state == MatchState.recap) //If game manager state is on 'Recap' (AH)
        {
            recapScreen.SetActive(true); //Show Recap Screen (AH)
            HideUIElements(); //Hide other certain UI elements (AH)
            ShowMatches(); //Show matches made (AH)
        }
        else
        {
            recapScreen.SetActive(false); //Hide Recap Screen (AH)
        }

        if (manager.state == MatchState.reset) //If game manager state is on 'Reset' (AH)
        {
            ResetMatching(); //reset matching (AH)
        }
    }

    private void HideUIElements()
    {
        for (int i = 0; i < hideUI.Count; i++)
        {
            hideUI[i].SetActive(false); //For each element in 'hideUI' hide them (AH)
        }

        if (checkMatchInt == 0) 
        {
            backButton.SetActive(false); //DO NOT show back button if you can not go further back in matches (AH)
        }
        else
        {
            backButton.SetActive(true); //show back button (AH)
        }

        if (checkMatchInt == matchCards.Count - 1)
        {
            nextButton.SetActive(false); //DO NOT show next button if you can not go further forward in matches (AH)
        }
        else
        {
            nextButton.SetActive(true); //show next button (AH)
        }
    }

    private void ShowMatches()
    {
        for (int i = 0; i < matchCards.Count; i++)
        {
            if (i == checkMatchInt) //Show match card and choice card that appears in the list at the 'checkMatchInt' (AH)
            {
                matchCards[checkMatchInt].SetActive(true);
                choiceCards[checkMatchInt].SetActive(true);
            }
            else //hide all other match and choice cards (AH)
            {
                matchCards[i].SetActive(false);
                choiceCards[i].SetActive(false);
            }

            textPercentNumber.text = "" + compatability[checkMatchInt]; //Update compatability based on the 'checkMatchInt' (AH)
        }
    }

    private void ResetMatching()
    {
        mainCamera.transform.position = new Vector3(0, 0, -10); //Main camera goes back to the hub screen (AH)
        background.SetActive(false);

        if (matchCards != null)
        {
            for (int i = 0; i < matchCards.Count; i++)
            {
                Destroy(choiceCards[i].gameObject);
                Destroy(matchCards[i].gameObject); //Destroy all cards (AH)
            } 

            compatability.Clear();
            choiceCards.Clear();
            matchCards.Clear(); //Clear all lists (AH)
        }

        checkMatchInt = 0; 
        matchSetUp.addedCards = false;
        matching.timer = matching.assignTimer;
        matching.score = 0;
        matching.scoreText.text = "Score: " + matching.score;
        //reset  all variables(AH)

        manager.state = MatchState.hub; //Changes game manager state to 'Hub' (AH)
    }

    public void NextButton()
    {
        if (checkMatchInt < matchCards.Count - 1)
        {
            checkMatchInt++; //increase checkMatchInt variable by 1 (AH)
        }
    }

    public void BackButton()
    {
        if (checkMatchInt > 0)
        {
            checkMatchInt--; //decrease checkMatchInt variable by 1 (AH)
        }
    }

    public void HomeButton()
    {
        enclosurePreview.panda = false;
        enclosurePreview.penguin = false;
        enclosurePreview.enclosureStage = 0;
        manager.state = MatchState.reset; //Change game manager state to 'Reset' (AH)
    }
}
