using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
	public Text score;
	private void Start()
	{
        IScoreManager l_IScoreManager = GameController.GetGameController().GetDependencyInjector().GetDependency<IScoreManager>();
		l_IScoreManager.scoreChangedDelegate += updateScore;
        updateScore(l_IScoreManager);
	}
	private void OnDestroy()
	{
		GameController.GetGameController().GetDependencyInjector().GetDependency<IScoreManager>().scoreChangedDelegate -= updateScore;
	}
	public void updateScore(IScoreManager scoreManager)
	{
		score.text = scoreManager.getPoints().ToString("0");
	}
}
