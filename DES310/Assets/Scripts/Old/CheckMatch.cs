using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckMatch : MonoBehaviour
{

    ThisCard thisCard;
    public GameObject matchZone;
    public GameObject matcheeZone;
    public GameObject stage1;
    public GameObject stage2;
    public bool matched;
    public List<GameObject> StageList;
    private int currentActiveIndex = 0;
    
    

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        //MatchCheck();
    }
    // Update is called once per frame
    public void MatchCheck()
    {
        ThisCard thisCard = gameObject.GetComponentInChildren<ThisCard>();
        if (thisCard.thisId == matchZone.GetComponentInChildren<ThisCard>().thisId)
        {
            matched = true;
            StageList[currentActiveIndex].SetActive(false);
            currentActiveIndex++;
            if (currentActiveIndex >= StageList.Count)
                currentActiveIndex = 0;
            StageList[currentActiveIndex].SetActive(true);
            Debug.Log("MATCH BABY");
            
            
        }
        

    }
}
