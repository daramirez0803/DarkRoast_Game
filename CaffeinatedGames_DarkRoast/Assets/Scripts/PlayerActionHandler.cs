using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerActionHandler : MonoBehaviour
{
    public static PlayerActionHandler instance { get; private set; }
    private Animator animator;
    private bool canQueueAction;
    private Vector3 desiredDirection;
    public bool isTurning;
    public float rotationSpeed = 30f;
    public float rotationDeadline;
    public GameObject weapon;
    private PlayerStaminaBar playerStaminaBar;

    public CapsuleCollider hitbox; 

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        hitbox = GetComponentsInChildren<CapsuleCollider>().Single(k => k.gameObject.name == "Hitbox");
        playerStaminaBar = GetComponent<PlayerStaminaBar>();
    }

    void OnEnable() {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("More than one PlayerActionHandler instance in the scene!");
            Destroy(gameObject);
        }
    }


    void LateUpdate() {
        if (isTurning) {
            Quaternion targetRotation = Quaternion.LookRotation(desiredDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            if (transform.rotation == targetRotation || Time.time > rotationDeadline) {
                isTurning = false;
            }
        }
    }

    // We got an input to dodge. Check if the conditions are right for the input to be acknowledged, and if so queue the action in the animator.
    public void TryDodge()
    {
        if (animator.IsInTransition(0)) {
            if (animator.GetNextAnimatorStateInfo(0).IsTag("ready")) {
                animator.SetTrigger("Dodge");
            } else {
                return;
            }
        }        
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("ready") || animator.GetBool("CanCancel") || canQueueAction) {
            animator.SetTrigger("Dodge");
        }
        //Debug.Log("None of the above");
    }


    // We got an input to attack. Check if the conditions are right for the input to be acknowledged, and if so queue the action in the animator.
    public void TryAttack()
    {
        if (animator.IsInTransition(0)) {
            if (animator.GetNextAnimatorStateInfo(0).IsTag("ready")) {
                animator.SetTrigger("Attack");
            } else {
                return;
            }
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("ready") || animator.GetCurrentAnimatorStateInfo(0).IsTag("attack") || canQueueAction) {
            animator.SetTrigger("Attack");
        }
    }

    // ================================ The following functions are events called by animations. ===============================

    // Because of input chaining/queueing, when an action is processed the player may not be facing the direction they intend to.
    // For example, if a player is spamming dodge, we will repeatedly go from the Dodge state to itself, which doesn't allow the player to change their direction even if they move their stick.
    // Therefore this function is called at the beginning of every action to allow the player a single direction change.
    public void AllowTurning() {
        if (TargetLockHandler.instance.isTargetLocked && animator.IsInTransition(0) && (animator.GetNextAnimatorStateInfo(0).IsTag("attack") || animator.GetNextAnimatorStateInfo(0).IsTag("attack3")))
        {
            desiredDirection = (TargetLockHandler.instance.currentTarget.transform.position - transform.position).normalized;
            desiredDirection.y = 0;
            isTurning = true;
            rotationDeadline = Time.time + 0.2f;
        }
        else {
            if (PlayerInputHandler.instance.stickDirection.magnitude > 0.1f)
            {
                desiredDirection = PlayerLocomotionHandler.instance.GetDesiredMovementDirection(PlayerInputHandler.instance.stickDirection);
                // Because the rotation of the transform is being controlled by Root Motion (with bake into pose), we can't set the forward direction directly
                // Instead, we flag that it needs to be updated then we will do it in LateUpdate.
                isTurning = true;
                rotationDeadline = Time.time + 0.2f;
            }
        }
    }

    public void DodgeBeginIFrames()
    {
        hitbox.enabled = false;
    }

    public void DodgeEndIFrames()
    {
        hitbox.enabled = true;
    }

    // For smooth gameplay, we allow some animations to be cancelled by another action slightly before the full animation is actually done.
    public void CanCancel()
    {
        animator.SetBool("CanCancel", true);
    }

    // For smooth gameplay, we allow actions to be queued up slightly before they can actually be performed. Without this grace period, the player may lose an input if they try to act just before the current action is cancelable.
    // But with too much grace period, the player will accidentally double up on actions if they are mashing it. I've tuned it to be roughly the same feeling as Dark Souls.
    public void CanQueueAction()
    {
        canQueueAction = true;
    }

    public void OnAttack() {
        AllowTurning();
        animator.SetBool("CanCancel", false);
        animator.ResetTrigger("Dodge");
        animator.ResetTrigger("Attack");
        canQueueAction = false;
        playerStaminaBar.HandleAttacking();
    }    

    public void OnDodge() {
        AllowTurning();
        animator.SetBool("CanCancel", false);
        animator.ResetTrigger("Dodge");
        animator.ResetTrigger("Attack");
        canQueueAction = false;
        playerStaminaBar.HandleDodge();
    }

    void AttackHitboxActive(float attackMultiplier) {
        weapon.GetComponent<WeaponAttackHandler>().HitboxActive(attackMultiplier);
    }

    void AttackHitboxInactive() {
        weapon.GetComponent<WeaponAttackHandler>().HitboxInactive();
    }
}
