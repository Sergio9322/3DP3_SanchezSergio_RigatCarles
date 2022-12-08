using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarioHealth : MonoBehaviour, IRestartGameElement
{
    [SerializeField] int m_Lifes = 3;
    [SerializeField] int m_MaxLifes = 3;
    [SerializeField] int m_Health = 8;
    [SerializeField] int m_MaxHealth = 8;
    [SerializeField] GameObject m_HealthBar;
    float m_RespawnAnimationTime = 7f;
    [SerializeField] Image m_HealthBarImage, m_HealthBarBackgroundOverlay;
    float m_BackgroundAlpha = 0.5f;
    [SerializeField] Gradient m_HealthBarColour;

    [SerializeField] float m_DelayBetweenLife = 1f;
    [SerializeField] float m_RespawnDelay = 5f;
    [SerializeField] Animator animator;

    void Start()
    {
        m_Health = m_MaxHealth;
        GameController.GetGameController().AddRestartGameElement(this);
        InitialiseHealthBar();
        GameController.GetGameController().GetDependencyInjector().GetDependency<ILifeManager>().setLifes(m_Lifes);
    }

    public void TakeDamage(int damage)
    {
        animator.SetTrigger("getHit");
        m_HealthBar.SetActive(true);
        StartCoroutine(UpdateHealthBar(damage));
    }

    void InitialiseHealthBar()
    {
        m_HealthBarImage.fillAmount = 1;
        m_HealthBarImage.color = m_HealthBarColour.Evaluate(m_HealthBarImage.fillAmount);
        m_HealthBarBackgroundOverlay.color = m_HealthBarColour.Evaluate(m_HealthBarImage.fillAmount);
        m_HealthBarBackgroundOverlay.color = new Color(m_HealthBarBackgroundOverlay.color.r, m_HealthBarBackgroundOverlay.color.g, m_HealthBarBackgroundOverlay.color.b, m_BackgroundAlpha * m_HealthBarImage.fillAmount);
    }

    IEnumerator UpdateHealthBar(int damage)
    {
        int l_LifesToSubstract = m_Health - damage < 0 ? m_Health : damage;
        for (int i = 0; i < l_LifesToSubstract; i++)
        {
            m_HealthBarImage.fillAmount -= 1f / m_MaxHealth;
            m_HealthBarImage.color = m_HealthBarColour.Evaluate(m_HealthBarImage.fillAmount);
            m_HealthBarBackgroundOverlay.color = m_HealthBarColour.Evaluate(m_HealthBarImage.fillAmount);
            m_HealthBarBackgroundOverlay.color = new Color(m_HealthBarBackgroundOverlay.color.r, m_HealthBarBackgroundOverlay.color.g, m_HealthBarBackgroundOverlay.color.b, m_BackgroundAlpha * m_HealthBarImage.fillAmount);
            SubstractDamage();
            yield return new WaitForSeconds(m_DelayBetweenLife);
        }
        
    }

    void SubstractDamage()
    {
        m_Health -= 1;
        animator.SetInteger("health", m_Health);
        if (m_Health <= 0)
        {
            m_Lifes--;
            GameController.GetGameController().EvaluateCurrentLife(m_Lifes);
            GameController.GetGameController().RestartGame();
            GameController.GetGameController().GetDependencyInjector().GetDependency<ILifeManager>().setLifes(m_Lifes);
        }
        
    }

    public void ResetLifes()
    {
        GameController.GetGameController().GetDependencyInjector().GetDependency<ILifeManager>().setLifes(m_MaxLifes);
    }

    void RespawnHealthBar()
    {
        StartCoroutine(RespawnHealthBarCoroutine());
    }

    IEnumerator RespawnHealthBarCoroutine()
    {
        yield return new WaitForSeconds(m_RespawnDelay);
        m_HealthBar.GetComponent<Animation>().Play("RespawnHealthBarAnimation");
        yield return new WaitForSeconds(m_RespawnDelay/2);
        int l_LifesToAdd = m_MaxHealth - m_Health;
        for (int i = 0; i < l_LifesToAdd; i++)
        {
            m_HealthBarImage.fillAmount += 1f / m_MaxHealth;
            m_HealthBarImage.color = m_HealthBarColour.Evaluate(m_HealthBarImage.fillAmount);
            m_HealthBarBackgroundOverlay.color = m_HealthBarColour.Evaluate(m_HealthBarImage.fillAmount);
            m_HealthBarBackgroundOverlay.color = new Color(m_HealthBarBackgroundOverlay.color.r, m_HealthBarBackgroundOverlay.color.g, m_HealthBarBackgroundOverlay.color.b, m_BackgroundAlpha * m_HealthBarImage.fillAmount);
            yield return new WaitForSeconds(m_DelayBetweenLife);
        }
        yield return new WaitForSeconds(m_RespawnAnimationTime);
        m_HealthBar.SetActive(false);
        m_Health = m_MaxHealth;
    }

    public void RestartGame()
    {
        RespawnHealthBar();
    }
}
