using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour, IScoreManager
{
	[SerializeField] float points;
	public event ScoreChanged scoreChangedDelegate;

	void Awake()
	{
		Debug.Log("EIIII, l'SCORE sóc a: "+gameObject);
        GameController.GetGameController().GetDependencyInjector().AddDependency<IScoreManager>(this);
	}
	public void addPoints(float points)
	{
		this.points += points;
		scoreChangedDelegate?.Invoke(this);
	}
	public float getPoints() { return points; }
}