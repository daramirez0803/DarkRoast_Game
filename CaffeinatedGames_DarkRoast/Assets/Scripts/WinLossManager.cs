using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WinLossManager : MonoBehaviour
{
    public static WinLossManager Instance;
    [SerializeField] private GameObject _LossRetryFirst;
    [SerializeField] private GameObject _LossMenuFirst;
    [SerializeField] private GameObject _WinToMenu;

    private void Awake()
    {
        if (WinLossManager.Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GameOver()
    {
        WinLossUI ui = GetComponent<WinLossUI>();
        Time.timeScale = 0.0f;
        PlayerInputHandler.instance.enabled = false;

        if (ui != null)
        {
            ui.ToggleDeathPanel();
            MenuManager.instance.canPause = false;

            EventSystem.current.SetSelectedGameObject(_LossMenuFirst);
        }
    }

    public void GameWon()
    {
        WinLossUI ui = GetComponent<WinLossUI>();
        Time.timeScale = 0.0f;
        PlayerInputHandler.instance.enabled = false;

        if (ui != null)
        {
            ui.ToggleWinPanel();
            MenuManager.instance.canPause = false;

            EventSystem.current.SetSelectedGameObject(_WinToMenu);
        }
    }
}
