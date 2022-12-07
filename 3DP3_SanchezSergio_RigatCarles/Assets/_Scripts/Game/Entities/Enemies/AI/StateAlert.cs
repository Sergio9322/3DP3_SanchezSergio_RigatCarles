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

    [SerializeField] Transform eyes;

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
            if (!hasSeenPlayer) UpdateState();
            ChangeState();
        }
    }

    void Initialise()
    {
        initialised = true;
        totalRotated = 0.0f;
        animator.SetBool("walk", false);
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
            animator.SetBool("walk", true);
            initialised = false;
        }
    }

    bool seesPlayer()
    {
        Ray r = new Ray(eyes.position, eyes.forward);
        if (Physics.Raycast(r, out RaycastHit hitInfo, chaseRange, obstacleMask))
            if (hitInfo.collider.gameObject.tag == "PlayerFOV") return true;
        return false;
    }  

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(eyes.position, eyes.forward * chaseRange);
    }

    bool hearsPlayer()
    {
        return (eyes.position - player.transform.position).magnitude < hearDistance;
    }

    IEnumerator WaitToAttack()
    {
        animator.SetBool("alert", true);
        initialised = false;
        yield return new WaitForSeconds(timeToAttack);
        hasSeenPlayer = false;
        stateManager.SetState(State.ATTACK);
        animator.SetBool("attack", true);
    }
}
