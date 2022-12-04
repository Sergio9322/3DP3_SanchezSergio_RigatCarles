using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof(StateManager))]
public class StatePatrol : MonoBehaviour, IStateAI
{
    State state = State.PATROL;
    StateManager stateManager;

    MarioController player;
    NavMeshAgent agent;
    [SerializeField] LayerMask obstacleMask;
    [SerializeField] List<Transform> patrolTargets;
    [SerializeField] float patrolMinDistance = 0.4f;
    [SerializeField] float patrolSpeed = 2;
    [SerializeField] float patrolAcceleration = 4f;
    int currentPatrolTarget = 0;
    Animator animator;

    [Header("ALERT")]
    [SerializeField] float hearDistance = 15f;
    [SerializeField] float alertSpeedRotation;
    [SerializeField]float totalRotated = 0.0f;

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
            UpdateState();
            ChangeState();
        }
    }
    
    public void UpdateState()
    {
        if (agent.isStopped) agent.isStopped = false;
        agent.speed = patrolSpeed;
        agent.acceleration = patrolAcceleration;
        if (agent.hasPath && agent.remainingDistance < patrolMinDistance)
        {
            currentPatrolTarget++;
        }
        agent.SetDestination(patrolTargets[currentPatrolTarget % patrolTargets.Count].position);
    }

    public void ChangeState()
    {
        if (hearsPlayer())
        {
            //stateManager.SetState(State.ALERT);
            //totalRotated = 0.0f;
            //animatior.SetTrigger("alert");
        }
    }  

    bool hearsPlayer()
    {
        return (transform.position - player.transform.position).magnitude < hearDistance;
    }
}
