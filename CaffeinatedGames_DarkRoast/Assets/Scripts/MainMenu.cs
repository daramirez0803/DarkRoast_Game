//Followed the tutorial found here: https://youtu.be/DX7HyN7oJjE?si=_OtrXk9pgmABnb82

using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1.0f;
    }

    public void PlayGame()
    {
        //TODO: Change this later to reflect actual start level
        LevelStateController.prevLevel = "MainMenu";
        PersistentValues.instance.SetCurrencyToZero();
        SceneManager.LoadSceneAsync("CoffeeShop");   
    }

    public void Quit()
    {
        Debug.Log("Game Quitted");
        Application.Quit();
    }
}