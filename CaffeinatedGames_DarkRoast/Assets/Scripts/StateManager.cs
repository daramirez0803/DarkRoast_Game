using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateManager : MonoBehaviour
{
    public void ReloadCurrentScene()
    {
        Debug.Log(gameObject.name + ": Reloading scene...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ChangeSceneByName(string name)
    {
        if (name != null)
        {
            Debug.Log(gameObject.name + ": Changing scene to " + name + "...");
            SceneManager.LoadScene(name);
        }
    }
}
