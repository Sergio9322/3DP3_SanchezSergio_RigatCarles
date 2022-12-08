using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarManager : MonoBehaviour, IStarManager
{
	[SerializeField] float stars;
	public event StarChanged starChangedDelegate;

	void Awake()
	{
        GameController.GetGameController().GetDependencyInjector().AddDependency<IStarManager>(this);
	}
	public void addStars(float stars)
	{
		this.stars += stars;
		starChangedDelegate?.Invoke(this);
	}
	public void setStars(float stars)
	{
		this.stars = stars;
		starChangedDelegate?.Invoke(this);
	}
	public float getStars() { return stars; }
}