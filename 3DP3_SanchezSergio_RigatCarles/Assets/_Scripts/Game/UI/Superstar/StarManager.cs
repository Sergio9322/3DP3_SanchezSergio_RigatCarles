using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarManager : MonoBehaviour, IStarManager
{
	[SerializeField] float stars;
	public event StarChanged starChangedDelegate;

	void Awake()
	{
		Debug.Log("uuuuuuuuuuuuuui, l'STAR sóc a: "+gameObject);
        GameController.GetGameController().GetDependencyInjector().AddDependency<IStarManager>(this);
		Debug.Log("ADDED???");
	}
	public void addStars(float stars)
	{
		this.stars += stars;
		starChangedDelegate?.Invoke(this);
	}
	public float getStars() { return stars; }
}