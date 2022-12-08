using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour, IRestartGameElement
{
	public Text score, superstars, lifes;
	float m_TimeToStartAnimation = 9.5f;
	bool m_HasStartedAnimation = false;
	float m_WaitToEnterStartPanel = 4f;
	GameOverPanel m_GameOverPanel;
	[SerializeField] Animation m_StartPanelAnimation;

	[SerializeField] bool m_HasDied;
	[SerializeField] bool m_HasLostLife;

	private void Start()
	{
        InitialiseScore();
		InitialiseSuperstars();
		InitialiseLifes();
		GameController.GetGameController().AddRestartGameElement(this);
		m_GameOverPanel = FindObjectOfType<GameOverPanel>();
	}

	void InitialiseScore()
	{
		IScoreManager l_IScoreManager = GameController.GetGameController().GetDependencyInjector().GetDependency<IScoreManager>();
		l_IScoreManager.scoreChangedDelegate += updateScore;
        updateScore(l_IScoreManager);
	}

	void InitialiseSuperstars()
	{
		IStarManager l_IStarManager = GameController.GetGameController().GetDependencyInjector().GetDependency<IStarManager>();
		l_IStarManager.starChangedDelegate += updateSuperstars;
		updateSuperstars(l_IStarManager);
	}

	void InitialiseLifes()
	{
		ILifeManager l_ILifeManager = GameController.GetGameController().GetDependencyInjector().GetDependency<ILifeManager>();
		l_ILifeManager.lifeChangedDelegate += updateLifes;
		updateLifes(l_ILifeManager);
	}

	private void Update()
	{
		if (!m_HasStartedAnimation) CheckStartAnimation();
	}

	private void OnDestroy()
	{
		GameController.GetGameController().GetDependencyInjector().GetDependency<IScoreManager>().scoreChangedDelegate -= updateScore;
	}
	public void updateScore(IScoreManager scoreManager)
	{
		score.text = scoreManager.getPoints().ToString("0");
	}

	public void updateSuperstars(IStarManager starManager)
	{
		superstars.text = starManager.getStars().ToString("0");
	}

	public void updateLifes(ILifeManager lifeManager)
	{
		lifes.text = lifeManager.getLifes().ToString("0");
	}

	void CheckStartAnimation()
	{
		m_TimeToStartAnimation -= Time.deltaTime;
		if (m_TimeToStartAnimation <= 0)
		{
			GetComponent<Animation>().Play("EnterUIAnimation");
			m_HasStartedAnimation = true;
		}
	}

	public void RestartGame()
	{
		m_TimeToStartAnimation = 6.5f;
		m_HasStartedAnimation = false;
	}

	public void PrepareHUDToRestartGame()
	{
		GetComponent<Animation>().Play("ExitUIAnimation");

		if (m_HasDied) { m_GameOverPanel.ShowGameOverPanel(); m_HasDied = false; StartCoroutine(WaitToEnterStartPanel());}
        else if (m_HasLostLife) { m_GameOverPanel.ShowLostLifePanel(); m_HasLostLife = false; StartCoroutine(WaitToEnterStartPanel());}
		else m_StartPanelAnimation.Play("ReenterStartPanelAnimation");
	}

	IEnumerator WaitToEnterStartPanel()
	{
		yield return new WaitForSeconds(m_WaitToEnterStartPanel);
		m_StartPanelAnimation.Play("StartAnimation");
	}
}
