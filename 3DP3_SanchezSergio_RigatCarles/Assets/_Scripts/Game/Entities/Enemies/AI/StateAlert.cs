using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAlert : MonoBehaviour, IStateAI
{
    State state = State.ALERT;
    StateManager stateManager;
    MarioController player;
    UnityEngine.AI.NavMeshAgent agent;
    Animator animator;
    bool initialised = false;
    float totalRotated = 0.0f;
    bool hasSeenPlayer = false;

    [SerializeField] LayerMask obstacleMask;
    [SerializeField] float hearDistance = 15f;
    [SerializeField] float alertSpeedRotation;
    [SerializeField] float chaseRange;
    [SerializeField] float timeToAttack = 0.7f;

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
            if (!hasSeenPlayer) UpdateState();
            ChangeState();
        }
    }

    void Initialise()
    {
        initialised = true;
        totalRotated = 0.0f;
    }
    
    public void UpdateState()
    {
        agent.isStopped = true;
        float frameRotation = alertSpeedRotation * Time.deltaTime;
        transform.Rotate(new Vector3(0.0f, alertSpeedRotation * Time.deltaTime, 0.0f));
        totalRotated += frameRotation;
    }

    public void ChangeState()
    {
        if (seesPlayer())
        {
            hasSeenPlayer = true;
            StartCoroutine(WaitToAttack());
        }
        if (!hearsPlayer() || totalRotated >= 360.0f)
        {
            stateManager.SetState(State.PATROL);
            animator.SetTrigger("walk");
            initialised = false;
        }
    }

    bool seesPlayer()
    {
        Ray r = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(r, out RaycastHit hitInfo, chaseRange, obstacleMask))
            if (hitInfo.collider.gameObject.tag == "Player") return true;
        return false;
    }  

    bool hearsPlayer()
    {
        return (transform.position - player.transform.position).magnitude < hearDistance;
    }

    IEnumerator WaitToAttack()
    {
        animator.SetTrigger("alert");
        initialised = false;
        yield return new WaitForSeconds(timeToAttack);
        stateManager.SetState(State.ATTACK);
        animator.SetTrigger("attack");
        hasSeenPlayer = false;
    }
}
