using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TecnoCampusDebugScaler : MonoBehaviour
{
    public KeyCode m_SlowKeyCodeDebug = KeyCode.LeftShift;
    public KeyCode m_FastKeyCodeDebug = KeyCode.LeftControl;

#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyDown(m_SlowKeyCodeDebug))
            Time.timeScale = 0.1f;
        if (Input.GetKeyUp(m_SlowKeyCodeDebug))
            Time.timeScale = 1.0f;    
        if (Input.GetKeyDown(m_FastKeyCodeDebug))
            Time.timeScale = 10.0f;
        if (Input.GetKeyUp(m_SlowKeyCodeDebug))
            Time.timeScale = 1.0f;
    }
#endif
}
