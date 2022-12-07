using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour, IRestartGameElement
{
	public Text score, superstars;
	float m_TimeToStartAnimation = 9.5f;
	bool m_HasStartedAnimation = false;

	private void Start()
	{
        InitialiseScore();
		InitialiseSuperstars();
		GameController.GetGameController().AddRestartGameElement(this);
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
}
