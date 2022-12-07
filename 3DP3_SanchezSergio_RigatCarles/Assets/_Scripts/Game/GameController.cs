using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    static GameController m_GameController;
    List<IRestartGameElement> m_RestartGameElements = new List<IRestartGameElement>();
    DependencyInjector m_DependencyInjector;

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

    private void Init(GameObject l_GameObject)
    {
        m_DependencyInjector = l_GameObject.AddComponent<DependencyInjector>();
        gameObject.AddComponent<ScoreManager>();
        gameObject.AddComponent<StarManager>();
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
        foreach (IRestartGameElement l_RestartGameElement in m_RestartGameElements)
            l_RestartGameElement.RestartGame();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            RestartGame();
    }
}
