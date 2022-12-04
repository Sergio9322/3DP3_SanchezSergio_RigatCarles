using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAttack : MonoBehaviour, IStateAI
{
    State state = State.ATTACK;
    StateManager stateManager;
    MarioController player;
    UnityEngine.AI.NavMeshAgent agent;
    Animator animator;
    bool initialised = false;

    [SerializeField] float chaseRange;
    [SerializeField] float chaseSpeed;

    void Awake()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        stateManager = GetComponent<StateManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<MarioController>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (stateManager.IsState(state))
        {
            if (!initialised) Initialise();
            UpdateState();
            ChangeState();
        }
    }

    void Initialise()
    {
        initialised = true;
    }
    
    public void UpdateState()
    {
        agent.isStopped = false;
        agent.SetDestination(player.transform.position);
    }

    public void ChangeState()
    {
        agent.speed = chaseSpeed;
        this.transform.LookAt(player.transform);
        if ((transform.position - player.transform.position).magnitude > chaseRange)
        {
            stateManager.SetState(State.PATROL);
            animator.SetTrigger("walk");
            initialised = false;
        }
    }  
}
