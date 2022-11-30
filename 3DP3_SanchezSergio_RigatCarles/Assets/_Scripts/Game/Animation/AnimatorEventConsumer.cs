using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEventConsumer : MonoBehaviour
{

    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip sound_jump1;
    [SerializeField] AudioClip sound_jump2;
    [SerializeField] AudioClip sound_jump3;
    [SerializeField] AudioClip sound_2jump;
    [SerializeField] AudioClip sound_3jump1;
    [SerializeField] AudioClip sound_3jump2;
    [SerializeField] AudioClip sound_3jump3;
    [SerializeField] AudioClip sound_step;
    void Step(int foot)
    {
        audioSource.PlayOneShot(sound_step);
    }
    void Jump1()
    {
        AudioClip chosen;
        int audioChosen = UnityEngine.Random.Range(1, 4);
        switch (audioChosen)
        {
            case 1: chosen = sound_jump1; break;
            case 2: chosen = sound_jump2; break;
            default: chosen = sound_jump3; break;
        }
        audioSource.PlayOneShot(chosen);
    }
    void Jump2()
    {
        audioSource.PlayOneShot(sound_2jump);
    }
    void Jump3()
    {
        AudioClip chosen;
        int audioChosen = UnityEngine.Random.Range(1, 4);
        switch (audioChosen)
        {
            case 1: chosen = sound_3jump1; break;
            case 2: chosen = sound_3jump2; break;
            default: chosen = sound_3jump3; break;
        }
        audioSource.PlayOneShot(chosen);
    }
    void walk()
    {

    }

    void PunchSound1(AnimationEvent _AnimationEvent)
    {

    }
    void PunchSound2(AnimationEvent _AnimationEvent)
    {


    }
    void PunchSound3(AnimationEvent _AnimationEvent)
    {

    }
    void FinishPunch(AnimationEvent _AnimationEvent)
    {

    }
}
