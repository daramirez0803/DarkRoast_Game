using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawnController : MonoBehaviour
{
    // public SkinnedMeshRenderer playerMesh;
    public bool spawnReset = false;
    public CharacterController playerController;
    // Start is called before the first frame update
    void Start()
    {  
        // playerMesh.enabled = false;
        spawnReset = false;  
        playerController.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        ResetSpawn();
        EnablePlayer();
        // if (SceneManager.GetActiveScene().name == "City" || SceneManager.GetActiveScene().name == "GasStation") 
        // {
        //     playerMesh.enabled = true;
        // }
        // else if (!playerMesh.enabled && 
        // (LevelStateController.prevLevel == "CoffeeShop" || LevelStateController.prevLevel == "City"))
        // {
        //     playerMesh.enabled = true;
        // }
    }

    private void EnablePlayer()
    {
        // if (LevelStateController.prevLevel == "CoffeeShop" && !playerMesh.enabled)
        // {
        //     // playerMesh.enabled = true;
        // }
        if (!playerController.enabled)
        playerController.enabled = true;
    }

    private void ResetSpawn()
    {
        if (!spawnReset && LevelStateController.prevLevel == "GasStation")
        {
            Debug.Log("Updating Spawn to " + LevelStateController.prevLevel);
            spawnReset = true;
            this.transform.position = LevelSpawnController.gasStationExit;
            // playerMesh.enabled = true;
            playerController.enabled = true;
        }
        playerController.enabled = true;

       if (LevelStateController.prevLevel == "MainMenu")
        {
            this.transform.position = new Vector3(-2, 0.1f, -2);
        }
    }
}
