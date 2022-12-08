using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Superstar : MonoBehaviour, IRestartGameElement, IPickable
{
    bool m_HasStartedDeactivation = false;
    float m_DeactivationSpeed = 4.0f;
    [SerializeField] Material m_LockedMaterial, m_UnlockedMaterial;
    [SerializeField] GameObject m_MyStar, m_MyCounter;

    void Start()
    {
        GameController.GetGameController().AddRestartGameElement(this);
    }

    public void Pick()
    {
        if (!m_HasStartedDeactivation)
        {
            StartCoroutine(Deactivate());
            GameController.GetGameController().GetDependencyInjector().GetDependency<IStarManager>().addStars(1);
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
        ChangeColours();
        GameController.GetGameController().RestartGame();
        m_HasStartedDeactivation = false;
    }

    void ChangeColours()
    {
        m_MyStar.GetComponent<Renderer>().material = m_LockedMaterial;
        m_MyCounter.GetComponent<Renderer>().material = m_UnlockedMaterial;
    }

    public void RestartStar()
    {
        m_MyStar.GetComponent<Renderer>().material = m_UnlockedMaterial;
        m_MyCounter.GetComponent<Renderer>().material = m_LockedMaterial;
    }

    public void RestartGame()
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.one;
    }
}
