using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    [SerializeField] State currentState = State.PATROL;

    public bool IsState(State state) { return currentState == state; }
    public void SetState(State state) { currentState = state; }
}
