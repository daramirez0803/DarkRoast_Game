using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// A Singleton class for handling player input. It maps button/controller input via InputSystem to variables.
// Other scripts can access an instance of this class via PlayerInputHandler.instance.
public class PlayerInputHandler : MonoBehaviour
{
    public static PlayerInputHandler instance { get; private set; }
    private PlayerControls playerControls;
    
    public Vector2 stickDirection { get; private set; }
    public Vector2 cameraStickDirection { get; private set; }
    [SerializeField] bool attackInput = false;
    [SerializeField] bool dodgeInput = false;
    [SerializeField] bool targetLockInput = false;
    private bool hasFocus = true;

    // Start is called before the first frame update
    void Start()
    {
        instance.enabled = true;
    }

    void OnApplicationFocus(bool focus) {
        if (focus && this.enabled) {
            Cursor.lockState = CursorLockMode.Locked;
        } else {
            Cursor.lockState = CursorLockMode.None;
        }
        hasFocus = focus;
    }

    void OnDisable() {
        Cursor.lockState = CursorLockMode.None;
    }

    void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.LogError("More than one PlayerInputHandler instance in the scene!");
            Destroy(gameObject);
            return;
        }

        if (playerControls == null) {
            playerControls = new PlayerControls();
        }
        playerControls.PlayerMovement.Movement.performed += ctx => stickDirection = ctx.ReadValue<Vector2>();
        playerControls.PlayerMovement.Movement.canceled += ctx => stickDirection = ctx.ReadValue<Vector2>();
        playerControls.PlayerActions.Attack.performed += ctx => attackInput = true;
        playerControls.PlayerActions.Dodge.performed += ctx => dodgeInput = true;
        playerControls.PlayerCamera.TargetLock.performed += ctx => targetLockInput = true;
        playerControls.PlayerCamera.MoveCamera.performed += ctx => cameraStickDirection = ctx.ReadValue<Vector2>();
        playerControls.PlayerCamera.MoveCamera.canceled += ctx => cameraStickDirection = Vector2.zero;
        playerControls.Enable();

        if (hasFocus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void Awake()
    {
        SceneManager.activeSceneChanged += OnSceneChange;
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChange;
    }

    private void OnSceneChange(Scene oldScene, Scene newScene)
    {
        // Handle scene change. If the new scene is the game scene, then enable player input. Otherwise, disable it.
        if (newScene.buildIndex == gameObject.scene.buildIndex) {
            instance.enabled = true;
        } else {
            instance.enabled = false;
        }
    }

    void Update()
    {
        PlayerLocomotionHandler.instance.HandleMovement(stickDirection);

        if (dodgeInput)
        {
            dodgeInput = false;
            PlayerActionHandler.instance.TryDodge();
        }
        else if (attackInput)
        {
            attackInput = false;
            PlayerActionHandler.instance.TryAttack();
        }

        if (targetLockInput)
        {
            targetLockInput = false;
            TargetLockHandler.instance.TargetLock();
        }
    }
}
