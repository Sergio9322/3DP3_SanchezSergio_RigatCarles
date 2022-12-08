using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    static GameController m_GameController;
    List<IRestartGameElement> m_RestartGameElements = new List<IRestartGameElement>();
    DependencyInjector m_DependencyInjector;

    [SerializeField] HUD m_HUD;
    float m_TimeToPrepareHud = 2f;
    float m_TimeToEnablePlayerMovement = 8.5f;
    float m_TimeToReenablePlayerMovement = 5.5f;

    static public GameController GetGameController()
    {
        if (m_GameController == null)
        {
            GameObject l_GameObject = new GameObject("GameController");
            GameController.DontDestroyOnLoad(l_GameObject);
            m_GameController = l_GameObject.AddComponent<GameController>();
            m_GameController.Init(l_GameObject);
        }
        return m_GameController;
    }

    void Start()
    {
        StartCoroutine(WaitToEnablePlayerMovement());
    }

    private void Init(GameObject l_GameObject)
    {
        m_DependencyInjector = l_GameObject.AddComponent<DependencyInjector>();
        gameObject.AddComponent<ScoreManager>();
        gameObject.AddComponent<StarManager>();
        gameObject.AddComponent<LifeManager>();
        m_HUD = FindObjectOfType<HUD>();
    }

    public DependencyInjector GetDependencyInjector()
    {
        return m_DependencyInjector;
    }

    public void AddRestartGameElement (IRestartGameElement RestartGameElement)
    {
        m_RestartGameElements.Add(RestartGameElement);
    }

    public void RestartGame()
    {
        StartCoroutine(RestartGameCoroutine());
    }

    IEnumerator RestartGameCoroutine()
    {
        MarioController f_MarioController = FindObjectOfType<MarioController>();
        f_MarioController.enabled = false;
        f_MarioController.SetAnimatorSpeedToZero();

        m_HUD.PrepareHUDToRestartGame();
        yield return new WaitForSeconds(m_TimeToPrepareHud);
        foreach (IRestartGameElement l_RestartGameElement in m_RestartGameElements)
            l_RestartGameElement.RestartGame();
        yield return new WaitForSeconds(m_TimeToReenablePlayerMovement);
        f_MarioController.enabled = true;
    }

    IEnumerator WaitToEnablePlayerMovement()
    {
        MarioController f_MarioController = FindObjectOfType<MarioController>();
        f_MarioController.enabled = false;
        f_MarioController.SetAnimatorSpeedToZero();
        yield return new WaitForSeconds(m_TimeToEnablePlayerMovement);
        f_MarioController.enabled = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            RestartGame();
    }
}
