using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour, IRestartGameElement, IPickable
{
    bool m_HasStartedDeactivation = false;
    float m_DeactivationSpeed = 4.0f;
    float m_TimeToDeactivate = 1f;
    [SerializeField] int hpGiven = 1;
    [SerializeField] AudioSource m_HeartAudioSource;
    [SerializeField] ParticleSystem m_HeartParticleSystem;
    [SerializeField] MarioHealth marioh;

    void Start()
    {
        GameController.GetGameController().AddRestartGameElement(this);
    }

    public void Pick()
    {
        if (!m_HasStartedDeactivation)
        {
            m_HeartAudioSource.Play();
            m_HeartParticleSystem.Play();
            StartCoroutine(Deactivate());
            marioh.GetHp(hpGiven);
        }
    }

    IEnumerator Deactivate()
    {
        m_HasStartedDeactivation = true;
        float l_Scale = 1.0f;
        while (l_Scale > 0.0f)
        {
            l_Scale -= Time.deltaTime * m_DeactivationSpeed;
            transform.localScale = new Vector3(l_Scale, l_Scale, l_Scale);
            yield return null;
        }
        yield return new WaitForSeconds(m_TimeToDeactivate);
        gameObject.SetActive(false);
        m_HasStartedDeactivation = false;
    }

    public void RestartGame()
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.one;
    }
}
