using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStarManager
{
	void addStars(float f);
	float getStars();
	event StarChanged starChangedDelegate;
}

public delegate void StarChanged(IStarManager starManager);