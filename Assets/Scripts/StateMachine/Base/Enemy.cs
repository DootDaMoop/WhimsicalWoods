using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IEnemyMoveable, ITriggerCheckable
{
    [field: SerializeField] public float maxHealth {get; set;} = 100f;
    public float currentHealth {get; set;}
    public Rigidbody2D rb { get; set; }
    public bool isFacingRight { get; set; } = false;
    public bool collisionHit {get; set;} = false;
    public bool isAggro { get; set; }
    public bool isWithinAttackRadius { get; set; }
    public float AggroRadius {get; set;} = 8f;
    public float AttackRadius {get; set;} = 2f;
    public GameObject player;
    public bool isHit;

    #region State Machine Usage
    public EnemyStateMachine StateMachine {get; set;}
    public EnemyIdleState IdleState {get; set;}
    public EnemyChaseState ChaseState {get; set;}
    public EnemyAttackState AttackState {get; set;}
    public Animator animator {get; set;}

    #endregion

    private void Awake() {
        StateMachine = new EnemyStateMachine();
        IdleState = new EnemyIdleState(this, StateMachine);
        ChaseState = new EnemyChaseState(this, StateMachine);
        AttackState = new EnemyAttackState(this, StateMachine);
    }
    private void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();

        StateMachine.Initalize(IdleState);
    }

    private void Update() {
        StateMachine.currentEnemyState.FrameUpdate();

        if(player != null) {
            if(playerDistance().magnitude <= AggroRadius) {
                setAggroStatus(true);
            }
            else {
                setAggroStatus(false);
            }

            if(playerDistance().magnitude <= AttackRadius) {
                setIsWithinAttackRadius(true);
            }
            else {
                setIsWithinAttackRadius(false);
            }
        }
        else {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, 8f);
    }

    #region Idle Vars
    public float randomMovementRange = 5f;
    public float randomMovementSpeed = 1f;
    
    #endregion

    #region Damage and Death
    public void Damage(float damageAmount)
    {
        currentHealth -= damageAmount;
        animator.SetTrigger("Damage");

        if(currentHealth <= 0) {
            animator.SetBool("DeadState",true);
            Death();
        }
    }

    public void Death()
    {
        Destroy(gameObject,1f);
    }

    #endregion

    #region Enemy Movement
    public void MoveEnemy(Vector2 velocity)
    {
        rb.velocity = velocity;
        CheckForLeftOrRightFacing(velocity);
    }

    public void CheckForLeftOrRightFacing(Vector2 velocity)
    {
        if(isFacingRight && velocity.x < 0f) {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;
        }
        else if(!isFacingRight && velocity.x > 0f) {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;
        }
    }

    #endregion

    #region Animation Triggers

    public void AnimationTriggerEvent(AnimationTriggerType triggerType) {
        StateMachine.currentEnemyState.AnimationTriggerEvent(triggerType);
    }

    public enum AnimationTriggerType {
        EnemyIdle,
        EnemyChase,
        EnemyAttack,
        EnemyDamaged,
        EnemyDeath
    }

    #endregion

    #region Collisions
    
    private void OnCollisionEnter2D(Collision2D other) {
        collisionHit = true;
    }

    private void OnCollisionStay2D(Collision2D other) {
        collisionHit = true;
    }

    #endregion

    #region Trigger Checks

    public void setAggroStatus(bool aggroStatus)
    {
        isAggro = aggroStatus;
    }

    public void setIsWithinAttackRadius(bool _isWithinAttackRadius)
    {
        isWithinAttackRadius = _isWithinAttackRadius;
    }

    public Vector3 playerDistance() {
        return transform.position - player.transform.position;
    }

    #endregion

    #region Attack Checks

        

    #endregion
}
