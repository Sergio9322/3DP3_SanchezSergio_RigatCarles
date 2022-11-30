using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, IRestartGameElement
{
    void Start()
    {
        GameController.GetGameController().AddRestartGameElement(this);
    }

    public void Pick()
    {
        GameController.GetGameController().GetDependencyInjector().GetDependency<IScoreManager>().addPoints(1);
        gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        gameObject.SetActive(true);
    }
}
