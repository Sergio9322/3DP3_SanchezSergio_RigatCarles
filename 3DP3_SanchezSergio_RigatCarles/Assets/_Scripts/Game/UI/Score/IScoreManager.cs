using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IScoreManager
{
	void addPoints(float f);
	float getPoints();
	void setPoints(float f);
	event ScoreChanged scoreChangedDelegate;
}

public delegate void ScoreChanged(IScoreManager scoreManager);