using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{

    public DialogueController dialogueController;
    public string curSceneName;
    private PlayerControls playerControls;

    // Start is called before the first frame update
    void Start()
    {
        curSceneName = SceneManager.GetActiveScene().name;
        if (playerControls == null) {
            playerControls = new PlayerControls();
        }
        playerControls.Menu.Confirm.performed += ctx => ConfirmPressed();
        playerControls.Enable();

    }

    void OnDisable()
    {
        if (playerControls != null) playerControls.Disable();
    }

    void OnDestroy()
    {
        if (playerControls != null) playerControls.Disable();
    }

    void ConfirmPressed()
    {
        if (dialogueController.exitDialogue)
        {
            if (curSceneName == "CoffeeShop")
            {
                Debug.Log(gameObject.name + ": loading city scene...");
                SceneManager.LoadSceneAsync("City");
                LevelStateController.tutorialComplete = true;
                LevelStateController.prevLevel = "CoffeeShop";
                Debug.Log(LevelStateController.tutorialComplete);
            }
            if (curSceneName == "City")
            {
                Debug.Log("Entered Gas Station");

                LevelSpawnController.gasStationExit = this.transform.position;
                LevelStateController.prevLevel = "City";
                Debug.Log(LevelSpawnController.gasStationExit);
                Debug.Log(gameObject.name + ": loading gas station...");
                SceneManager.LoadSceneAsync("GasStation");

            }
            if (curSceneName == "GasStation")
            {
                Debug.Log("Entered City");
                LevelStateController.prevLevel = "GasStation";
                SceneManager.LoadSceneAsync("City");

            }

        }
    }
}
