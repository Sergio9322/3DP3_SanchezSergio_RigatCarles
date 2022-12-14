using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour, IScoreManager
{
	[SerializeField] float points;
	public event ScoreChanged scoreChangedDelegate;

	void Awake()
	{
        GameController.GetGameController().GetDependencyInjector().AddDependency<IScoreManager>(this);
	}
	public void addPoints(float points)
	{
		this.points += points;
		scoreChangedDelegate?.Invoke(this);
	}
	public void setPoints(float points) 
	{ 
		this.points = points; 
		scoreChangedDelegate?.Invoke(this);
	}
	public float getPoints() { return points; }
}