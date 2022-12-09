using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Impulsable : MonoBehaviour
{
    [SerializeField] float m_HitDuration = 0.5f;
    [SerializeField] float m_HitForce = 10f;
    NavMeshAgent m_NavMeshAgent;
    MarioController m_MarioController;
    [SerializeField] AudioSource m_AudioSource;

    void Start()
    {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        m_MarioController = GetComponent<MarioController>();
    }
    public void GetImpulsed(Vector3 l_Direction)
    {
        StartCoroutine(Impulse(l_Direction));
    }

    private IEnumerator Impulse(Vector3 l_Direction)
    {
        if(m_AudioSource!=null) m_AudioSource.Play();
        DisableOtherMovements();
        float l_Time = 0.0f;
        float l_Gravity = 6f;
        Vector3 l_InitialPosition = transform.position;
        while (l_Time < m_HitDuration)
        {
            l_Time += Time.deltaTime;
            transform.position = l_InitialPosition + l_Direction * m_HitForce * l_Time;
            transform.position += Vector3.up * (l_Time < m_HitDuration / 2 ? l_Time : m_HitDuration - l_Time) * l_Gravity;
            yield return null;
        }
        EnableOtherMovements();
    }
    
    private void DisableOtherMovements()
    {
        if (m_NavMeshAgent != null) m_NavMeshAgent.enabled = false;
        if (m_MarioController != null) m_MarioController.enabled = false;
    }

    private void EnableOtherMovements()
    {
        if (m_NavMeshAgent != null) m_NavMeshAgent.enabled = true;
        if (m_MarioController != null) m_MarioController.enabled = true;
    }
}