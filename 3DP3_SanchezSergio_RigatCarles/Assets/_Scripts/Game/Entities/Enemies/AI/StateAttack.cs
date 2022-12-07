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
    Vector3 chaseDirection;

    [SerializeField] float chaseRange;
    [SerializeField] float chaseSpeed;
    float maxFinalDestinationRange = 10000f;

    void Awake()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        stateManager = GetComponent<StateManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<MarioController>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (stateManager.IsState(state) && agent.enabled)
        {
            if (!initialised) Initialise();
            UpdateState();
            ChangeState();
        }
    }

    void Initialise()
    {
        initialised = true;
        agent.isStopped = false;
        agent.speed = chaseSpeed;
        this.transform.LookAt(player.transform);
        agent.ResetPath();
        chaseDirection = (player.transform.position - this.transform.position).normalized;
        animator.SetBool("alert", false);
    }
    
    public void UpdateState()
    {
        if (agent.enabled)
            agent.Move(chaseDirection * chaseSpeed * Time.deltaTime);
    }

    public void ChangeState()
    {
        if ((transform.position - player.transform.position).magnitude > chaseRange)
        {
            stateManager.SetState(State.PATROL);
            animator.SetBool("walk", true);
            initialised = false;
        }
    }  
}
