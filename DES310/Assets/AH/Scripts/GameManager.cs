using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MatchState
{
    setUp,
    calculate,
    matching,
    reset,
    recap
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
