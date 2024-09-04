using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance { get; private set; }
    public GameObject pauseMenuPanel;
    public GameObject optionsPanel;
    public GameObject statsPanel;
    public GameObject controlsPanel;
    public GameObject creditsPanel;
    public GameObject mainMenuPanel;
    public GameObject onScreenText;
    private GameObject[] allPanels;
    public bool canPause = true;

    private bool didCloseText = false;

    private bool GameIsPaused;
    private PlayerControls playerControls;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("More than one instance of MenuManager found!");
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.LogError("More than one instance of MenuManager found!");
        }
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
        }
        playerControls.Menu.Pause.performed += ctx => PausePressed();
        playerControls.Enable();
    }

    void Start()
    {
        allPanels = new GameObject[] { pauseMenuPanel, optionsPanel, statsPanel, controlsPanel, creditsPanel, mainMenuPanel };
        Time.timeScale = 1.0f;
        GameIsPaused = false;
    }

    public void ShowMenu(GameObject menu) {
        EventSystem.current.SetSelectedGameObject(null);
        for (int i = 0; i < allPanels.Length; i++)
        {
            if (allPanels[i] == null) {
                continue;
            }
            if (allPanels[i] == menu) {
                allPanels[i].SetActive(true);
            } else {
                allPanels[i].SetActive(false);
            }
        }
    }

    private void CloseAllMenus() {
        ShowMenu(null);
    }

    public void OpenOptions()
    {
        ShowMenu(optionsPanel);
    }

    public void OpenStats()
    {
        ShowMenu(statsPanel);
    }

    public void OpenControls()
    {
        ShowMenu(controlsPanel);
    }

    public void OpenPause()
    {
        ShowMenu(pauseMenuPanel);
    }

    public void OpenCredits()
    {
        ShowMenu(creditsPanel);
    }

    public void OpenMainMenu()
    {
        ShowMenu(mainMenuPanel);
    }


    private void PausePressed()
    {
        if (!canPause) {
            return;
        }
        if (GameIsPaused)
        {
            Resume();
        } else if (!GameIsPaused)
        {
            Pause();
        }
    }

    public void Resume()
    {
        UIManager.Instance.ShowHUD();
        if (onScreenText != null && didCloseText) {
            onScreenText.SetActive(true);
        }
        GameIsPaused = false;
        Time.timeScale = 1.0f;
        PlayerInputHandler.instance.enabled = true;
        CloseAllMenus();
    }

    private void Pause()
    {
        UIManager.Instance.HideHUD();
        if (onScreenText != null) {
            if (onScreenText.activeSelf) {
                onScreenText.SetActive(false);
                didCloseText = true;
            }
        }
        GameIsPaused = true;
        Time.timeScale = 0.0f;
        PlayerInputHandler.instance.enabled = false;
        OpenPause();
    }
}