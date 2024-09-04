using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class TargetLockHandler : MonoBehaviour
{
    public static TargetLockHandler instance { get; private set; }

    public float maxScanAngle = 75;
    public float maxRange = 15;

    public bool isTargetLocked = false;
    public GameObject currentTarget;
    private Animator animator;

    public LayerMask candidateLayer;
    public Transform playerCamera;
    public Animator cameraAnimator;
    public Cinemachine.CinemachineVirtualCamera targetLockCamera;
    public PlayerLocomotionHandler locomotion;
    public Camera mainCamera;

    public Texture2D dotTexture; // Assign a white dot texture in the inspector
    private int dotRadius = 7;

    private Vector2 cameraStickDirection;

    PlayerControls playerControls;
    bool targetSwitchReset = false;

    private void Start()
    {
        locomotion = GetComponent<PlayerLocomotionHandler>();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        if (dotTexture == null)
        {
            dotTexture = new Texture2D(2 * dotRadius, 2 * dotRadius);
            DrawCircle(dotTexture, Color.white, dotRadius, dotRadius, dotRadius);
            dotTexture.Apply();
        }

        if (playerControls == null) {
            playerControls = new PlayerControls();
        }
        playerControls.PlayerCamera.MoveCamera.performed += ctx => cameraStickDirection = ctx.ReadValue<Vector2>();
        playerControls.PlayerCamera.MoveCamera.canceled += ctx => cameraStickDirection = ctx.ReadValue<Vector2>();
        playerControls.Enable();
    }

    void OnGUI()
    {
        if (currentTarget == null) return;
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(currentTarget.GetComponent<Collider>().bounds.center);
        Rect rect = new Rect(screenPosition.x - dotRadius, Screen.height - screenPosition.y - dotRadius, dotRadius * 2, dotRadius * 2);
        GUI.DrawTexture(rect, dotTexture);
    }

    public Texture2D DrawCircle(Texture2D tex, Color color, int x, int y, int radius = 3)
    {
        float rSquared = radius * radius;

        for (int i = 0; i < tex.width; i++) {
            for (int j = 0; j < tex.height; j++) {
                if ((x - i) * (x - i) + (y - j) * (y - j) <= rSquared) {
                    tex.SetPixel(i, j, color);
                } else {
                    tex.SetPixel(i, j, Color.clear);
                }
            }
        }

        return tex;
    }

    void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("More than one TargetLockHandler instance found.");
            Destroy(gameObject);
        }

    }

    void Update()
    {
        locomotion.lockedOn = isTargetLocked;

        if (isTargetLocked)
        {
            if (cameraStickDirection.magnitude < 0.1f) {
                targetSwitchReset = true;
            }
            if (cameraStickDirection.magnitude > 0.9f && targetSwitchReset) {
                SwitchTargets(cameraStickDirection);
                targetSwitchReset = false;
            }
            if (!IsValidTarget())
                    {
                        ClearTarget();
                    }
        }
    }

    public void TargetLock()
    {
        //Debug.Log("TargetLock Triggered");
        if (currentTarget != null)
            ClearTarget();
        else
        {
            currentTarget = Scan();
            if (currentTarget != null)
            {
                LockTarget();
            }
        }
    }

    private void SwitchTargets(Vector2 stickDirection) {
        float currentAngle;
        GameObject bestCandidate;
        float bestDistance = Mathf.Infinity;
        bestCandidate = null;

        Collider[] candidates;
        candidates = Physics.OverlapSphere(transform.position, maxRange, candidateLayer);

        Debug.Log("Switch Target: There are " + candidates.Length.ToString() + " objects in range");

        Vector2 currentTargetScreenPosition = mainCamera.WorldToScreenPoint(currentTarget.GetComponent<Collider>().bounds.center);
        for(int i = 0; i < candidates.Length; i++)
        {
            if (candidates[i].gameObject == currentTarget) continue;
            Vector2 candidateScreenPosition = mainCamera.WorldToScreenPoint(candidates[i].GetComponent<Collider>().bounds.center);

            Vector2 direction = candidateScreenPosition - currentTargetScreenPosition;
            currentAngle = Vector2.Angle(direction, stickDirection);
            if (currentAngle < 70f) {
                float distance = Vector2.Distance(candidateScreenPosition, currentTargetScreenPosition);
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    bestCandidate = candidates[i].gameObject;
                    Debug.Log("Best candidate is " + bestCandidate.name);
                }
            }
        }

        if (bestCandidate != null) {
            currentTarget = bestCandidate;
            LockTarget();
        }
        
    }

    //Scan method local variables
    private GameObject Scan()
    {
        float bestAngle;
        float currentAngle;
        GameObject bestCandidate;
        Vector3 direction;
        Collider[] candidates;
        candidates = Physics.OverlapSphere(transform.position, maxRange, candidateLayer);
        bestAngle = maxScanAngle;
        bestCandidate = null;
        Debug.Log("There are " + candidates.Length.ToString() + " objects in range");
        Vector3 cameraForward = playerCamera.forward; 
        Debug.DrawLine(transform.position, transform.position + cameraForward * maxRange, Color.red, 6f);

        for(int i = 0; i < candidates.Length; i++)
        {
            direction = candidates[i].transform.position - playerCamera.position;
            direction.y = 0;
            currentAngle = Vector3.Angle(cameraForward, direction);
            Debug.Log("Angle between player and " + candidates[i].name + " is " + currentAngle.ToString() + ". Direction is " + direction.ToString() + " and forward is " + cameraForward.ToString());
            if(currentAngle < bestAngle)
            {
                bestAngle = currentAngle;
                bestCandidate = candidates[i].gameObject;
                Debug.Log("Best candidate is " + bestCandidate.name);
            }
        }

        if (bestCandidate == null)
            return null;
        
        return bestCandidate;
    }

    void LockTarget()
    {
        isTargetLocked = true;
        targetLockCamera.LookAt = currentTarget.transform;
        var composer = targetLockCamera.GetCinemachineComponent<CinemachineComposer>();
        composer.m_TrackedObjectOffset = new Vector3(0, currentTarget.GetComponent<Collider>().bounds.size.y/2, 0);
        cameraAnimator.Play("TargetLockCamera");
        animator.SetBool("TargetLocked", true);
        locomotion.lockedOn = true;

    }

    void ClearTarget()
    {
        currentTarget = null;
        isTargetLocked = false;
        cameraAnimator.Play("PlayerCamera");
        targetLockCamera.LookAt = null;
        animator.SetBool("TargetLocked", false);
        locomotion.lockedOn = false;
    }


    bool IsValidTarget()
    {
        if (IsTargetDead())
        {
            return false;
        }

        return true;
    }

    private bool IsTargetDead()
    {
        if (currentTarget.GetComponent<EnemyStatusReporter>().IsDead())
        {
            return true;
        }

        return false;
    }

    //TODO: IMPLIMENT FOLLOWING FUNCTIONS
    private bool IsInRange()
    {
        return false;
    }

    private bool IsBlocked()
    {
        return false;
    }

}
