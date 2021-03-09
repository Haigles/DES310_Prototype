using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPool : MonoBehaviour
{
    [Space(5)]
    [Header("Panda Match Card Pool")]
    [Space(10)]
    [SerializeField] public CardInfo[] pandaMatchCards;

    [Space(5)]
    [Header("Panda Choice Card Pool")]
    [Space(15)]
    [SerializeField] public CardInfo[] pandaChoiceCards;

}

[System.Serializable]
public class CardInfo
{
    public string cardName;

    [Header("Strings")]
    public string cardAge;
    public string cardParents;
    public string cardLocation;
    public string cardHealth;

    [Header("Values")]
    public int valueAge;
    public string maleParent;
    public string femaleParent;
    public int valueLocation;
    public int valueHealth;
}
