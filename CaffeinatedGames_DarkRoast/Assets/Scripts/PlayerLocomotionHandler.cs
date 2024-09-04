using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotionHandler : MonoBehaviour
{
    public static PlayerLocomotionHandler instance { get; private set; }
    private TargetLockHandler targetLockHandler;
    private Camera mainCamera;
    [Range(10f, 80f)] public float rotationSpeed = 40f;
    private Animator animator;

    private CharacterController characterController;

    public bool lockedOn;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
        targetLockHandler = GetComponent<TargetLockHandler>();
        characterController = GetComponent<CharacterController>();
    }

    void OnEnable() {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("More than one PlayerLocomotionHandler instance found.");
            Destroy(gameObject);
        }
    }

    public Vector3 GetDesiredMovementDirection(Vector2 inputStickDirection) {
        Vector3 stickDirection = new Vector3(inputStickDirection.x, 0, inputStickDirection.y);
        Vector3 desiredDirection = mainCamera.transform.TransformDirection(stickDirection);
        desiredDirection.y = 0;
        return desiredDirection;
    }

    public void HandleMovement(Vector2 inputStickDirection)
    {
        // Always expose the stick magnitude to the animator. This is not used for movement but for state transitions.
        animator.SetFloat("StickMagnitude", inputStickDirection.magnitude);
        animator.SetFloat("Vertical", inputStickDirection.y);
        animator.SetFloat("Horizontal", inputStickDirection.x);

        // We don't want the player to rotate freely if they are in an action. So we only do locomotion if they are in a ready state or transitioning to one.
        if ((animator.IsInTransition(0) && animator.GetNextAnimatorStateInfo(0).IsTag("ready")) || (!animator.IsInTransition(0) && animator.GetCurrentAnimatorStateInfo(0).IsTag("ready")))
        {
            // TODO this doesn't work if the ground isn't flat
            if (inputStickDirection.magnitude > 0.1f || lockedOn )
            {
                Vector3 desiredDirection;
                if (lockedOn)
                {
                     desiredDirection = (targetLockHandler.currentTarget.transform.position - transform.position).normalized;
                     desiredDirection.y = 0;
                }
                else
                {
                    desiredDirection = GetDesiredMovementDirection(inputStickDirection);
                }

                transform.forward = Vector3.Lerp(transform.forward, desiredDirection, Time.deltaTime * rotationSpeed);
                animator.SetFloat("Speed", Vector3.ClampMagnitude(inputStickDirection, 1).magnitude);
            }
            else
            {
                animator.SetFloat("Speed", 0);
            }
        }
    }

    public void Update() {
        characterController.Move(-5 * Vector3.up * Time.deltaTime);
    }
}
