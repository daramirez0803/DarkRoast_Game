using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxManagerScript : MonoBehaviour
{
    public AudioSource Audio;
    public AudioClip Click;
    public static SfxManagerScript sfxInstance;


    private void Awake()
    {
        if (sfxInstance != null && sfxInstance != this)
        {
            Debug.LogError("More than one instance of SfxManagerScript found!");
            Destroy(this.gameObject);
            return;
        }

        // don't destroy between scenes
        sfxInstance = this;
        DontDestroyOnLoad(this);
    }
}
