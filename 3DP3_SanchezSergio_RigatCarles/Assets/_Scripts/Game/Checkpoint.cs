using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class Checkpoint : MonoBehaviour
{
    public Transform m_RespawnPosition;
    bool m_FirstTime = true;
    [SerializeField] AudioSource m_CheckpointAudioSource;
    [SerializeField] Material m_ActivatedMaterial;
    [SerializeField] SkinnedMeshRenderer m_MeshRenderer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (m_FirstTime)
            {
                m_FirstTime = false;
                FirstTimeActivation();
            }
            else RegularActivation();
        }
    }

    void RegularActivation()
    {
        GetComponent<Animation>().Play();
        m_CheckpointAudioSource.Play();
    }

    void FirstTimeActivation()
    {
        m_MeshRenderer.material = m_ActivatedMaterial;
        RegularActivation();
    }
}
