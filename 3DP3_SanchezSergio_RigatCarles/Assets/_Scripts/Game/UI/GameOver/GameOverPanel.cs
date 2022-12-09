using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] GameObject m_BowserMask, m_BowserImage, m_CircleImage, m_StarMask;
    float m_WaitingTimeBetweenMasks = 4f;
    [SerializeField] AudioSource m_BowserAudioSource;

    void Start()
    {
        m_BowserMask.SetActive(false);
        m_StarMask.SetActive(false);
    }

    public void ShowLostLifePanel()
    {
        ActivateCircleMask();
        StartCoroutine(FinishPanelCoroutine());
    }

    public void ShowGameOverPanel()
    {
        ActivateBowserMask();
        StartCoroutine(FinishPanelCoroutine());
        GameController.GetGameController().GameOverGame();

        m_BowserAudioSource.Play();
    }

    IEnumerator FinishPanelCoroutine()
    {
        yield return new WaitForSeconds(m_WaitingTimeBetweenMasks);
        ActivateStarMask();
        yield return new WaitForSeconds(m_WaitingTimeBetweenMasks);
        m_StarMask.SetActive(false);
    }
    
    void ActivateBowserMask()
    {
        m_BowserMask.SetActive(true);
        m_BowserImage.GetComponent<Image>().enabled = true;
        m_CircleImage.SetActive(false);
    }

    void ActivateCircleMask()
    {
        m_BowserMask.SetActive(true);
        m_BowserImage.GetComponent<Image>().enabled = false;
        m_CircleImage.SetActive(true);
    }

    void ActivateStarMask()
    {
        Debug.Log("ActivateStarMask");
        m_StarMask.SetActive(true);
        m_BowserMask.SetActive(false);
    }
}
