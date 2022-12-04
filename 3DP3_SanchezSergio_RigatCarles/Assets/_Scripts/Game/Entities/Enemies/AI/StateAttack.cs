using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAttack : MonoBehaviour, IStateAI
{
    [SerializeField] State state = State.ATTACK;
    public void UpdateState()
    {
        throw new System.NotImplementedException();
    }

    public void ChangeState()
    {
        throw new System.NotImplementedException();
    }  
}
