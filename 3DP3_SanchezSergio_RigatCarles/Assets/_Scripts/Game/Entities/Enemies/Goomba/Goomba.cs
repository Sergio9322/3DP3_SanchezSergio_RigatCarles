using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateManager))]
public class Goomba : MonoBehaviour
{
    bool m_CanDoDamage = true;
    [SerializeField] float m_DamageInterval = 1.0f;
    [SerializeField] int  m_Damage = 1;
    
    StateManager m_StateManager;
    bool m_Alive = true;
    public void SetAlive(bool l_Alive) { m_Alive = l_Alive; }

    void Awake()
    {
        m_StateManager = GetComponent<StateManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "MarioHit")
        {
            this.KillHitting();
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
}
