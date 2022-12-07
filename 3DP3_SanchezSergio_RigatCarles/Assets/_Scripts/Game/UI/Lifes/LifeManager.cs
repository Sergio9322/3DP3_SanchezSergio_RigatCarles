using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour, ILifeManager
{
	[SerializeField] float lifes;
	public event LifeChanged lifeChangedDelegate;

	void Awake()
	{
        GameController.GetGameController().GetDependencyInjector().AddDependency<ILifeManager>(this);
	}
	public void setLifes(float lifes)
	{
		this.lifes = lifes;
		lifeChangedDelegate?.Invoke(this);
	}
	public float getLifes() { return lifes; }
}