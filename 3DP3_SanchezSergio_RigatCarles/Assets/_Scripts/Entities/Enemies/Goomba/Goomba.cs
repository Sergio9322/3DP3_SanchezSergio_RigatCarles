using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : MonoBehaviour, IRestartGameElement
{
    public float m_DeadTime = 0.5f;
    void Start()
    {
        GameController.GetGameController().AddRestartGameElement(this);
    }
    public void Kill()
    {
        StartCoroutine(KillCoroutine());
    }
    IEnumerator KillCoroutine()
    {
        transform.localScale = new Vector3(1.0f, 0.1f, 1.0f);
        yield return new WaitForSeconds(m_DeadTime);
        gameObject.SetActive(false);
        // TODO: Animar amb el temps
    }
    public void RestartGame()
    {
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        gameObject.SetActive(true);
    }
}
