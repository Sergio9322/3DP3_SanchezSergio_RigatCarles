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

    public float m_DeadTime = 2f;
    public float m_TimeHitImpulseAndDie = 1f;
    bool m_Alive = true;

    [SerializeField] ParticleSystem m_DieParticles;
    [SerializeField] AudioSource m_AudioSource;
    [SerializeField] AudioClip m_DieAudioClip;

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
        float l_ScaleSpeed = 0.05f;
        while(transform.localScale.y > 0.1f)
        {
            transform.localScale = new Vector3(1.0f, transform.localScale.y - l_ScaleSpeed, 1.0f);
            yield return null;
        }
        yield return new WaitForSeconds(m_DeadTime);
        m_Alive = false;
        m_DieParticles.Play();
        m_AudioSource.PlayOneShot(m_DieAudioClip);
        gameObject.SetActive(false);
    }

    IEnumerator KillHittingCoroutine()
    {
        ImpulseFarAway();
        yield return new WaitForSeconds(m_TimeHitImpulseAndDie);
        float l_ScaleSpeed = 0.01f;
        while(transform.localScale.y > 0.1f)
        {
            transform.localScale = new Vector3(transform.localScale.x - l_ScaleSpeed , transform.localScale.y - l_ScaleSpeed, transform.localScale.z - l_ScaleSpeed);
            yield return null;
        }
        yield return new WaitForSeconds(m_DeadTime);
        gameObject.SetActive(false);
        m_Alive = false;
        m_DieParticles.Play();
        m_AudioSource.PlayOneShot(m_DieAudioClip);
    }

    void ImpulseFarAway()
    {
        Vector3 l_Direction = transform.position - player.transform.position;
        if (this.TryGetComponent(out Impulsable l_GoombaImpulsable))
            l_GoombaImpulsable.GetImpulsed(l_Direction);
    }

    public void RestartGame()
    {
        initialised = false;
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        gameObject.SetActive(true);
        m_Alive = true;
        animator.SetTrigger("restart");
    }
}
