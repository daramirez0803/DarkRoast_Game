using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgScript : MonoBehaviour
{
    public static BgScript BgInstance;

    private void Awake()
    {
        if(BgInstance != null && BgInstance != this)
        {
            Debug.LogError("More than one instance of BgScript found!");
            Destroy(this.gameObject);
            return;
        }

        // don't destroy between scenes
        BgInstance = this;
        DontDestroyOnLoad(this);
    }
}
