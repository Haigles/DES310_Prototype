using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MatchState
{
    hub,
    selection,
    setUp,
    calculate,
    matching,
    recap,
    reset
}
public class GameManager : MonoBehaviour
{
    public MatchState state;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
