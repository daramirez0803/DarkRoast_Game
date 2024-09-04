using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSoundHandler : MonoBehaviour
{
    private AudioSource movementSource;
    private AudioSource hitSource;
    private AudioSource vocalSource;

    [Header("Movement Sounds")]
    public AudioClip footStep;
    public AudioClip footStepDrag;

    [Header("Hit Sounds")]
    public AudioClip hitSound;

    [Header("Zombie Vocal Sounds")]
    public List<AudioClip> groanSounds;
    public List<AudioClip> idleSounds;
    public AudioClip screamSound;
    public AudioClip agonySound;
    public AudioClip deathSound;

    void Start()
    {
        movementSource = GetComponents<AudioSource>()[0];
        hitSource = GetComponents<AudioSource>()[1];
        vocalSource = GetComponents<AudioSource>()[2];
    }

    void PlayFootStep()
    {
        movementSource.clip = footStep;
        movementSource.volume = Random.Range(.6f, .9f);
        movementSource.pitch = Random.Range(.8f, 1.2f);

        movementSource.Play();
    }

    void PlayFootStepDrag()
    {
        movementSource.clip = footStepDrag;
        movementSource.volume = Random.Range(.85f, 1f);
        movementSource.pitch = Random.Range(.8f, 1.2f);

        movementSource.Play();
    }

    void PlayHitSound()
    {
        hitSource.clip = hitSound;
        hitSource.volume = 1f;
        hitSource.Play();
    }

    void PlayGroanSound(float chance)
    {
        float rand = Random.Range(0, .999f);
        if(rand < chance)
        {
            AudioClip clip = groanSounds[Random.Range(0, groanSounds.Count)];

            vocalSource.clip = clip;
            vocalSource.volume = .1f;
            vocalSource.Play();
        }
    }

    void PlayIdleSound()
    {
        AudioClip clip = idleSounds[Random.Range(0, idleSounds.Count)];

        vocalSource.clip = clip;
        vocalSource.volume = .1f;
        vocalSource.Play();
    }

    void PlayScreamSound()
    {
        vocalSource.clip = screamSound;
        vocalSource.volume = .1f;
        vocalSource.Play();
    }

    void PlayAgonySound()
    {
        vocalSource.clip = agonySound;
        vocalSource.volume = .1f;
        vocalSource.Play();
    }

    void PlayDeathSound()
    {
        vocalSource.clip = deathSound;
        vocalSource.volume = .1f;
        vocalSource.Play();
    }
}
