using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(StateManager))]
public class Goomba : MonoBehaviour, IRestartGameElement
{
    bool m_CanDoDamage = true;
    [SerializeField] float m_DamageInterval = 1.0f;
    [SerializeField] int  m_Damage = 1;
    Vector3 m_InitialPosition, m_InitialScale, m_InitialRotation;
    NavMeshAgent m_NavMeshAgent;
    StateManager m_StateManager;

    [Header("Hit")]
    [SerializeField] float m_HitDuration = 0.5f;

    bool m_Alive = true;
    public void SetAlive(bool l_Alive) { m_Alive = l_Alive; }

    void Awake()
    {
        m_StateManager = GetComponent<StateManager>();
    }

    void Start()
    {
        GameController.GetGameController().AddRestartGameElement(this);
        m_InitialPosition = transform.position;
        m_InitialScale = transform.localScale;
        m_InitialRotation = transform.eulerAngles;
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "MarioHit")
        {
            this.KillHitting();
        }
        if (other.GetComponent<Collider>().tag == "Player" )
        {
            MarioController l_MarioController = other.GetComponent<Collider>().GetComponent<MarioController>();
            if (this.IsAlive()) l_MarioController.GoombaHit(this);
        }
    }
    
    public void KillJumping() { m_StateManager.SetState(State.DIE_JUMP); }
    public void KillHitting() { m_StateManager.SetState(State.DIE_HIT); }


    public bool TryGetDamage()
    {
        if (m_CanDoDamage)
        {
            m_CanDoDamage = false;
            StartCoroutine(WaitUntilCanDoDamage());
            return true;
        }
        return false;
    }

    public bool IsAlive() { return m_Alive; }

    IEnumerator WaitUntilCanDoDamage()
    {
        yield return new WaitForSeconds(m_DamageInterval);
        m_CanDoDamage = true;
    }
    
    public int GetDamageAmount() { return m_Damage; }

    public void RestartGame()
    {
        m_Alive = true;
        transform.position = m_InitialPosition;
        transform.localScale = m_InitialScale;
        transform.eulerAngles = m_InitialRotation;
    }

    public void GetImpulsed(Vector3 l_Direction)
    {
        StartCoroutine(Impulse(l_Direction));
    }

    private IEnumerator Impulse(Vector3 l_Direction)
    {
        m_NavMeshAgent.enabled = false;
        float l_Time = 0.0f;
        float l_Gravity = 6f;
        Vector3 l_InitialPosition = transform.position;
        while (l_Time < m_HitDuration)
        {
            l_Time += Time.deltaTime;
            transform.position = l_InitialPosition + l_Direction * l_Time;
            transform.position += Vector3.up * (l_Time < m_HitDuration / 2 ? l_Time : m_HitDuration - l_Time) * l_Gravity;
            yield return null;
        }
        m_NavMeshAgent.enabled = true;
    }
}
