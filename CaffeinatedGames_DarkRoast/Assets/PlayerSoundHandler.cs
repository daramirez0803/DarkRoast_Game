using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundHandler : MonoBehaviour
{
    private AudioSource attackSource, movementSource;

    [Header("Attack Sounds")]
    public AudioClip attack1;
    public AudioClip attack2;
    public AudioClip attack3;

    [Header("Movement Sounds")]
    public AudioClip footStep;
    public AudioClip roll;

    void Start()
    {
        attackSource = GetComponents<AudioSource>()[0];
        movementSource = GetComponents<AudioSource>()[1];
    }

    void PlayAttack1()
    {
        attackSource.clip = attack1;
        attackSource.volume = 1f;
        attackSource.Play();
    }

    void PlayAttack2()
    {
        attackSource.clip = attack2;
        attackSource.volume = 1f;
        attackSource.Play();
    }

    void PlayAttack3()
    {
        attackSource.clip = attack3;
        attackSource.volume = 1f;
        attackSource.Play();
    }

    void PlayFootStep() 
    {
        movementSource.clip = footStep;
        movementSource.volume = Random.Range(.2f, .5f);
        movementSource.pitch = Random.Range(.8f, 1.2f);

        movementSource.Play();
    }

    void PlaySoftStep()
    {
        movementSource.clip = footStep;
        movementSource.volume = Random.Range(.1f, .3f);
        movementSource.pitch = Random.Range(.8f, 1.2f);

        movementSource.Play();
    }

    void PlayRoll()
    {
        movementSource.clip = roll;
        movementSource.volume = 1.5f;
        movementSource.Play();
    }
}
