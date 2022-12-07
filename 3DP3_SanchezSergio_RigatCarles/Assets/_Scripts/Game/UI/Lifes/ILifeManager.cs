using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILifeManager
{
	void setLifes(float f);
	float getLifes();
	event LifeChanged lifeChangedDelegate;
}

public delegate void LifeChanged(ILifeManager lifeManager);