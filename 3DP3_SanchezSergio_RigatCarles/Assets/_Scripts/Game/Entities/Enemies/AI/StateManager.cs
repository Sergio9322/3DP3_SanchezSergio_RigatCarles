using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour, IRestartGameElement
{
    [SerializeField] State currentState = State.PATROL;

    public bool IsState(State state) { return currentState == state; }
    public void SetState(State state) { currentState = state; CheckDied(state); }

    public void RestartGame()
    {
        currentState = State.PATROL;
        GetComponent<Goomba>().SetAlive(true);
    }

    void CheckDied(State state)
    {
        if (state == State.DIE_HIT || state == State.DIE_JUMP)
            GetComponent<Goomba>().SetAlive(false);
    }
}
