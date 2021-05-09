using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MatchState //All Game States (AH)
{
    hub,
    selection,
    setUp,
    calculate,
    matching,
    recap,
    reset,
    tutorial,
    countdown,
    startMenu
}
public class GameManager : MonoBehaviour //Controls which state the game is in (AH)
{
    public MatchState state;

    public void changeState(int newState)
    {
        state = (MatchState)newState;
    }
}
