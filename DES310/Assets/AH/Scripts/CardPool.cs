using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPool : MonoBehaviour
{
    [Space(5)]
    [Header("Tutorial Match Card Pool")]
    [Space(10)]
    [SerializeField] public CardInfo[] tutorialMatchCards;

    [Space(5)]
    [Header("Tutorial Choice Card Pool")]
    [Space(10)]
    [SerializeField] public CardInfo[] tutorialChoiceCards;

    [Space(5)]
    [Header("Panda Match Card Pool")]
    [Space(10)]
    [SerializeField] public CardInfo[] pandaMatchCards;

    [Space(5)]
    [Header("Panda Choice Card Pool")]
    [Space(15)]
    [SerializeField] public CardInfo[] pandaChoiceCards;

    [Space(5)]
    [Header("Penguin Match Card Pool")]
    [Space(15)]
    [SerializeField] public CardInfo[] penguinMatchCards;

    [Space(5)]
    [Header("Penguin Choice Card Pool")]
    [Space(15)]
    [SerializeField] public CardInfo[] penguinChoiceCards;

    [Space(5)]
    [Header("Giraffe Match Card Pool")]
    [Space(15)]
    [SerializeField] public CardInfo[] giraffeMatchCards;

    [Space(5)]
    [Header("Giraffe Choice Card Pool")]
    [Space(15)]
    [SerializeField] public CardInfo[] giraffeChoiceCards;

    [Space(5)]
    [Header("Lion Match Card Pool")]
    [Space(15)]
    [SerializeField] public CardInfo[] lionMatchCards;

    [Space(5)]
    [Header("Lion Choice Card Pool")]
    [Space(15)]
    [SerializeField] public CardInfo[] lionChoiceCards;

}

[System.Serializable]
public class CardInfo
{
    public string cardName;

    public Texture cardPicture;

    [Header("Strings")]
    public string cardAge;
    public string cardParents;
    public string cardDistance;
    public string cardHealth;

    [Header("Values")]
    [Range(0, 100)]
    public int valueAge;
    public string maleParent;
    public string femaleParent;
    [Range(0, 100)]
    public int valueDistance;
    [Range(0, 100)]
    public int valueHealth;
}
