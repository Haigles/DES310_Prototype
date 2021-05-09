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
    public TMP_Text comboCounter;
    public RecapDialogue recapDialogue = null;
    public TMP_Text dialogueText = null;
    private bool hasUpdatedDialogue = false;

    [Header("Card Positions")]
    [Space(10)]
    public Transform matchPosition;
    public Transform choicePosition;
      
    private int checkMatchInt = 0;
    public int comboStage = 0;
    private GameManager manager;
    public CameraHandler cameraHandler;
    private MatchSetUp matchSetUp;
    private Matching matching;
    

    [Header("Hide These UI Elements")]
    [Space(10)]
    [SerializeField]
    List<GameObject> hideUI;
    public GameObject background;

    [Header("Updates from Matching")]
    [Space(10)]
    public GameObject recapEnclosure = null;
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
        //mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.state == MatchState.recap) //If game manager state is on 'Recap' (AH)
        {
            for (int i = 0; i < matchCards.Count; i++)
            {
                matchCards[i].GetComponent<Draggable>().enabled = false;
            }
            for (int i = 0; i < choiceCards.Count; i++)
            {
                choiceCards[i].GetComponent<Draggable>().enabled = false;
            }

            recapScore.text = "" + matching.score;
            comboCounter.text = "Highest Combo: x" + comboStage;
            recapEnclosure.GetComponent<SpriteRenderer>().sprite = matchSetUp.enclosurePreview.GetComponent<SpriteRenderer>().sprite;
            recapEnclosure.GetComponent<RawImage>().texture = matchSetUp.enclosurePreview.GetComponent<RawImage>().texture;
            recapScreen.SetActive(true); //Show Recap Screen (AH)
            HideUIElements(); //Hide other certain UI elements (AH)
            ShowMatches(); //Show matches made (AH)
            UpdateDialogue();
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
        cameraHandler.ChangeCameraPosSelect();
        //mainCamera.transform.position = new Vector3(0, 0, -10); //Main camera goes back to the hub screen (AH)
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
        matching.comboStage = 0;
        //reset  all variables(AH)

        manager.state = MatchState.hub; //Changes game manager state to 'Hub' (AH)
    }

    private void UpdateDialogue()
    {
        CardDetails choiceCard = choicePosition.gameObject.GetComponentInChildren<CardDetails>();

        if (!hasUpdatedDialogue)
        {
            if (choiceCard.matchValue == 0)
            {
                int rng = Random.Range(0, 2);

                dialogueText.text = recapDialogue.dialogue[rng];

            }
            else if (choiceCard.matchValue == 1)
            {
                if (choiceCard.highAge)
                {
                    dialogueText.text = recapDialogue.dialogue[8];
                }
                else if (choiceCard.highLocation)
                {
                    dialogueText.text = recapDialogue.dialogue[3];
                }
                else if (choiceCard.highHealth)
                {
                    dialogueText.text = recapDialogue.dialogue[4];
                }
                else if (choiceCard.highParents)
                {
                    dialogueText.text = recapDialogue.dialogue[5];
                }
            }
            else if (choiceCard.matchValue == 2)
            {
                if (choiceCard.highAge)
                {
                    dialogueText.text = recapDialogue.dialogue[8];
                }
                else if (choiceCard.highLocation)
                {
                    dialogueText.text = recapDialogue.dialogue[3];
                }
                else if (choiceCard.highHealth)
                {
                    dialogueText.text = recapDialogue.dialogue[6];
                }
                else if (choiceCard.highParents)
                {
                    dialogueText.text = recapDialogue.dialogue[7];
                }
            }

            hasUpdatedDialogue = true;
        }
    }

    public void NextButton()
    {
        if (checkMatchInt < matchCards.Count - 1)
        {
            checkMatchInt++;//increase checkMatchInt variable by 1 (AH)
        }

        hasUpdatedDialogue = false;
    }

    public void BackButton()
    {
        if (checkMatchInt > 0)
        {
            checkMatchInt--; //decrease checkMatchInt variable by 1 (AH)
        }

        hasUpdatedDialogue = false;
    }

    public void HomeButton()
    {
        enclosurePreview.panda = false;
        enclosurePreview.penguin = false;
        enclosurePreview.enclosureStage = 0;
        manager.state = MatchState.reset; //Change game manager state to 'Reset' (AH)
    }
}
