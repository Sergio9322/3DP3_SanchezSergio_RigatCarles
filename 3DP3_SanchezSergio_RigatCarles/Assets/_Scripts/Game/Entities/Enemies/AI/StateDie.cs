using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateDie : MonoBehaviour, IStateAI, IRestartGameElement
{
    State killedJumpingState = State.DIE_JUMP;
    State killedHittingState = State.DIE_HIT;
    StateManager stateManager;
    MarioController player;
    UnityEngine.AI.NavMeshAgent agent;
    Animator animator;
    bool initialised = false;

    public float m_DeadTime = 0.5f;
    bool m_Alive = true;

    void Awake()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        stateManager = GetComponent<StateManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<MarioController>();
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        GameController.GetGameController().AddRestartGameElement(this);
    }

    void Update()
    {
        if (stateManager.IsState(killedJumpingState) || stateManager.IsState(killedHittingState))
        {
            if (!initialised) Initialise();
            UpdateState();
            ChangeState();
        }
    }

    void Initialise()
    {
        initialised = true;
        animator.SetTrigger("die");
        if (stateManager.IsState(killedJumpingState))
            StartCoroutine(KillJumpingCoroutine());
        else if (stateManager.IsState(killedHittingState))
            StartCoroutine(KillHittingCoroutine());
    }
    
    public void UpdateState()
    {
        
    }

    public void ChangeState()
    {
        
    }

    IEnumerator KillJumpingCoroutine()
    {
        transform.localScale = new Vector3(1.0f, 0.1f, 1.0f);
        yield return new WaitForSeconds(m_DeadTime);
        gameObject.SetActive(false);
        m_Alive = false;
        // TODO: Animar amb el temps
        // TODO: Particles
    }

    IEnumerator KillHittingCoroutine()
    {
        // TODO: Fer sortir disparat
        yield return new WaitForSeconds(m_DeadTime);
        gameObject.SetActive(false);
        m_Alive = false;
        // TODO: Particles
    }

    public void RestartGame()
    {
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        gameObject.SetActive(true);
        m_Alive = true;
        animator.SetTrigger("restart");
    }
}
