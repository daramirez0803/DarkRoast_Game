using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuNavigation : MonoBehaviour
{
    private PlayerControls playerControls;
    private Selectable currentButton;
    public Selectable firstSelectable;
    public GameObject previousMenu;
    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
        }
        playerControls.Menu.MenuMove.performed += ctx => MenuMove(ctx.ReadValue<Vector2>());
        playerControls.Menu.Back.performed += ctx => GoBack();
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void MenuMove(Vector2 ctx) {
        Debug.Log(gameObject.name + ": " + EventSystem.current.currentSelectedGameObject);
        if (EventSystem.current.currentSelectedGameObject == null) {
            EventSystem.current.SetSelectedGameObject(firstSelectable.gameObject);
        }
    }

    private void GoBack() {
        if (previousMenu != null) {
            MenuManager.instance.ShowMenu(previousMenu);
        }
    }
}
