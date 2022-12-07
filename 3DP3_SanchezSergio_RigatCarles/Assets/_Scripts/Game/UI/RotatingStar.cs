using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingStar : MonoBehaviour
{
    [SerializeField] float m_RotationSpeed = 4f;
    void Update()
    {
        Rotate();
    }

    void Rotate()
    {
        transform.Rotate(0, 0, m_RotationSpeed);
    }
}
