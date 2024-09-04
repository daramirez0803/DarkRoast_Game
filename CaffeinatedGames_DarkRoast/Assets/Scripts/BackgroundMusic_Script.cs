//This script keeps the background music playing from scene to scene.
//Followed the tutorial found here: https://youtu.be/63BEZMjcegE?si=32nZPkYQfPO2psaX

using UnityEngine;

public class BackgroundMusic_Script : MonoBehaviour
{
    public static BackgroundMusic_Script instance;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = null;
        }
        else
        {
            Debug.LogError("More than one instance of BackgroundMusic_Script found!");
            Destroy(gameObject);
        }
    }
}