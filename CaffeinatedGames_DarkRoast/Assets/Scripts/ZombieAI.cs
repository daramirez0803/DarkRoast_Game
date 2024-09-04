using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(LayerMask))]
public class ZombieAI : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent nav;
    private Animator animator;
    private Transform player;
    private EnemyStatusReporter statusReporter;
    private bool isAttacking;
    private bool queueAttack = false;
    private bool isTakingDamage = false;
    private bool queueTakingDamage = false;
    private bool isDead = false;
    private bool pauseBeforeMove;
    private float pauseTime;
    
    
    public GameObject rightHand;
    public LayerMask targetLayer;
    public int currencyValue = 100;
    public float noticeRange = 10;
    public float health;
    public float maxHealth = 50;
    public float attackRange = 1f;

    //AI Variables
    private int currentPatrolTarget;
    public float baseSpeed = 1;
    public AIState aiState;
    public float patrolSpeed = 1;
    public GameObject[] patrolPoints;
    public enum AIState
    {
        Idle,
        Patrol,
        TrackTarget,
        AttackTarget,
        TakeDamage,
        Die
    }


    private Camera mainCamera;

    void OnGUI()
    {
        if (health <= 0 || (health == maxHealth && !GameObject.ReferenceEquals(TargetLockHandler.instance.currentTarget, this.gameObject))) {
            return;
        }
        Bounds bounds = GetComponent<Collider>().bounds;
        Vector3 worldPosition = new Vector3(bounds.center.x, bounds.max.y, bounds.center.z) + Vector3.up * 0.3f;
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);
        if (screenPosition.z < 0) {
            return;
        }
        Vector2 healthBarSize = new Vector2(80, 5);

        Rect healthBarRect = new Rect(screenPosition.x - healthBarSize.x / 2, Screen.height - screenPosition.y - healthBarSize.y / 2, healthBarSize.x, healthBarSize.y);
        GUI.color = Color.red;
        GUI.DrawTexture(healthBarRect, Texture2D.whiteTexture);

        float healthPercentage = health / maxHealth;
        Rect healthFillRect = new Rect(healthBarRect.x, healthBarRect.y, healthBarRect.width * healthPercentage, healthBarRect.height);
        GUI.color = Color.green;
        GUI.DrawTexture(healthFillRect, Texture2D.whiteTexture);

    }

    void Start()
    {
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();
        statusReporter = GetComponent<EnemyStatusReporter>();
        nav.speed = baseSpeed;
        mainCamera = Camera.main;
        health = maxHealth;

        if (patrolPoints.Length == 0)
        {
            aiState = AIState.Idle;
        }
        else
        {
            aiState = AIState.Patrol;
            currentPatrolTarget = 0;
            nav.speed = patrolSpeed;
            pauseBeforeMove = true;
            pauseTime = Random.Range(1, 4);
        }
    }


    void Update()
    {
        if (health <= 0)
            aiState = AIState.Die;

        //if our nav is ever moving, reflect it on our zombie
        if(!isDead)
            animator.SetFloat("speed",nav.velocity.magnitude);

        switch (aiState)
        {
            case AIState.Idle:
                CheckNearbyPlayer();

                break;

            case AIState.Patrol:
                if (pauseBeforeMove)
                {
                    pauseTime -= Time.deltaTime;
                    //Debug.Log(pauseTime);
                    if (pauseTime < 0)
                    {
                        pauseBeforeMove = false;
                        SetNextWaypoint();
                    } 
                    else
                    {
                        return;
                    }
                }
                if(Vector3.Distance(transform.position, patrolPoints[currentPatrolTarget].transform.position) < .6)
                {
                    SetNextWaypoint();
                }
                CheckNearbyPlayer();

                break;

            case AIState.TrackTarget:
                nav.SetDestination(player.position);
                //transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

                float distance = Vector3.Distance(transform.position, player.position);

                //target left max notice range
                if (distance > noticeRange)
                {
                    player = null;
                    animator.SetFloat("speed", 0);
                    nav.ResetPath();

                    //determine what state to return to
                    if (patrolPoints.Length == 0)
                    {
                        aiState = AIState.Idle;
                    }
                    else
                    {
                        aiState = AIState.Patrol;
                        nav.speed = patrolSpeed;
                        nav.SetDestination(patrolPoints[currentPatrolTarget].transform.position);
                    }
                }

                //target is within attack range
                if(distance < attackRange)
                {
                    //nav.ResetPath();
                    //animator.SetFloat("speed", 0);
                    queueAttack = true;
                    isAttacking = true;
                    aiState = AIState.AttackTarget;
                }

                break;

            case AIState.AttackTarget:

                transform.LookAt(player);
                if (queueAttack)
                {
                    float randAttack = Random.Range(0,4);
                    nav.speed = nav.speed * 7;  //speed up the zombie movement on attack to feel better

                    //"randomly" do a certain attack, weighted for more consistent play experience
                    if (randAttack < 1.5)
                    {
                        animator.SetTrigger("attack_swing");
                    }
                    else if (randAttack < 2.5)
                    {
                        animator.SetTrigger("attack_jab");
                    }
                    else if (randAttack < 3.25)
                    {
                        animator.SetTrigger("attack_overhead");
                    }
                    else
                    {
                        animator.SetTrigger("attack_heavy");
                    }

                    queueAttack = false;
                }

                if (!isAttacking)
                {
                    //if our nav speed was changed for an attack then revert to our base speed
                    if (nav.speed != baseSpeed)
                    {
                        nav.speed = baseSpeed;
                        Debug.Log("RESETTING NAVE SPEED TO: " + nav.speed);
                    }

                    aiState = AIState.TrackTarget;
                }

                break;

            case AIState.TakeDamage:

                if (queueTakingDamage)
                {
                    nav.isStopped = true;
                    animator.SetTrigger("hit");
                    queueTakingDamage = false;
                }
                

                if (!isTakingDamage)
                {
                    aiState = AIState.Idle;
                    nav.isStopped = false;
                }

                break;

            case AIState.Die:
                if (!isDead)
                {
                    isDead = true;
                    statusReporter.SetStatusDead();
                    gameObject.layer = 0; //default layer, makes the dead zombie no longer targetable
                    animator.SetTrigger("death");
                    
                    gameObject.GetComponent<CapsuleCollider>().enabled = false; //remove the capsule collider on death

                    PersistentValues.instance.currency += currencyValue;
                    UIManager.Instance.UpdateCurrencyText();

                }
                
                break;
        }
    }

    private void CheckNearbyPlayer()
    {
        Collider[] playerTargets = Physics.OverlapSphere(transform.position, noticeRange, targetLayer);

        for (int i = 0; i < playerTargets.Length; i++)
        {
            player = playerTargets[i].transform;
        }

        if (player != null)
        {
            aiState = AIState.TrackTarget;
            nav.speed = baseSpeed;
        }
    }

    private void SetNextWaypoint()
    {
        if(patrolPoints.Length != 0)
        {
            if (currentPatrolTarget == patrolPoints.Length - 1)
            {
                currentPatrolTarget = 0;
            }
            else
            {
                currentPatrolTarget += 1;
            }

            nav.SetDestination(patrolPoints[currentPatrolTarget].transform.position);
        }
    }

    public void TakeDamage(float dmg)
    {
        aiState = AIState.TakeDamage;
        //TAKE DAMAGE
        health -= dmg;
        queueTakingDamage = true;
        isTakingDamage = true;
    }

    //Events will be triggered by animator
    public void AttackEnded()
    {
        isAttacking = false;
        //Debug.Log("Attack ended");
    }

    public void HitReactionEnded()
    {
        isTakingDamage = false;
        //Debug.Log("Hit reaction ended");
    }

    public void RightHandActive(float damage) {
        rightHand.GetComponent<ZombieWeaponController>().RightHandActive(damage);
    }


    public void RightHandInactive() {
        rightHand.GetComponent<ZombieWeaponController>().RightHandInactive();
    }
}
