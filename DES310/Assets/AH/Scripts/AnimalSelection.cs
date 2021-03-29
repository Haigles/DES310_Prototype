using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AnimalSelection : MonoBehaviour
{
    GameManager manager;
    public GameObject animalSelectionMenu;

    [SerializeField]
    public List<bool> animals;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectAnimal(int animalIndex)
    {
        for (int i = 0; i < animals.Count; i++)
        {
            animals[i] = false;
        }

        animals[animalIndex] = true;

        manager.state = MatchState.setUp;
        animalSelectionMenu.SetActive(false);
    }
}
