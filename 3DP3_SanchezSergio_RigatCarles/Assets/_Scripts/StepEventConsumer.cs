using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepEventConsumer : MonoBehaviour
{
    void Step(int foot)
    {
        Debug.Log("step: "+foot);
    }
}
