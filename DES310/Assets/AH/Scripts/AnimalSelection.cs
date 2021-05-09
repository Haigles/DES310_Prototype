using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AnimalSelection : MonoBehaviour
{
    GameManager manager;
    Matching matching;

    public GameObject animalSelectionMenu;
    public SelectEnclosure selectedEnclosure;
    public GameObject stageEnclosurePrefab;

    [SerializeField]
    public List<bool> animals;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        matching = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Matching>();
    }

    public void SelectAnimal(int animalIndex)
    {
        for (int i = 0; i < animals.Count; i++)
        {
            animals[i] = false; //Sets all animals to false to begin with (AH)
        }

        animals[animalIndex] = true; //Chooses which animal is being matched, based on which button is pressed (AH)

        selectedEnclosure.canPick = false;
        matching.stageEnclosure = selectedEnclosure.gameObject;
        ResizeEnclosure(selectedEnclosure.gameObject);
        manager.state = MatchState.setUp; //Chamges game manager state to 'SetUp' (AH)      
        animalSelectionMenu.SetActive(false); //Hide selection menu (AH);
    }

    public void CloseMenu()
    {
        manager.state = MatchState.hub; //Chamges game manager state to 'Hub' (AH)
        animalSelectionMenu.SetActive(false); //Hide selection menu (AH);
        selectedEnclosure = null; //Clears selected enclosure (AH)
    }

    public void ResizeEnclosure(GameObject enclosure)
    {
        enclosure.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        enclosure.transform.localPosition += selectedEnclosure.newOffset;
    }
}
