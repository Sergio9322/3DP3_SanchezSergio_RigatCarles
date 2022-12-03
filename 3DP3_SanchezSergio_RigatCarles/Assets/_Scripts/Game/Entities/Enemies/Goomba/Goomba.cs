using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : MonoBehaviour, IRestartGameElement
{
    public float m_DeadTime = 0.5f;
    bool m_Alive = true;
    bool m_CanDoDamage = true;
    [SerializeField] float m_DamageInterval = 1.0f;

    void Start()
    {
        GameController.GetGameController().AddRestartGameElement(this);
    }
    public void Kill()
    {
        StartCoroutine(KillCoroutine());
    }
    IEnumerator KillCoroutine()
    {
        transform.localScale = new Vector3(1.0f, 0.1f, 1.0f);
        yield return new WaitForSeconds(m_DeadTime);
        gameObject.SetActive(false);
        m_Alive = false;
        // TODO: Animar amb el temps
    }
    public void RestartGame()
    {
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        gameObject.SetActive(true);
        m_Alive = true;
    }

    public bool TryGetDamage()
    {
        if (m_CanDoDamage)
        {
            m_CanDoDamage = false;
            StartCoroutine(WaitUntilCanDoDamage());
            return true;
        }
        Debug.Log("No damage");
        return false;
    }
    public bool IsAlive() { return m_Alive; }

    IEnumerator WaitUntilCanDoDamage()
    {
        yield return new WaitForSeconds(m_DamageInterval);
        m_CanDoDamage = true;
    }
}
