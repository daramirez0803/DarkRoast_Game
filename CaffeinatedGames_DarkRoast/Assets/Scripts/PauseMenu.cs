//Tutorial follow from here: https://youtu.be/JivuXdrIHK0?si=AVJXWoyt7p55Qhdg

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    private void Start()
    {
    }

    public void OpenMainMenu()
    {
        Debug.Log("Loading menu...");
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}