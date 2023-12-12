using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEnum : MonoBehaviour
{
    // Universal Variables
    private EnemyState currentState;
    [SerializeField] private float maxHealth = 100f;
    public float currentHealth;
    [SerializeField] private float attackDamage = 10f;
    public Rigidbody2D rb;
    public Animator animator;
    public GameObject player;
    public bool isFacingRight;
    public bool isRanged = false;
    public bool collisionHit {get; set;} // Collision with wall
    public bool playerHit {get; set;} // Collision with Player
    public bool isAggro {get; set;}
    public bool isAttacking {get; set;}
    public float aggroRadius = 8f;
    public float attackRadius = 3f;
    public float movementSpeed = 1.75f;
    [SerializeField] private float nextAttackTime = 0f;
    [SerializeField] private float attackCooldown = 1f;
    public GameObject projectilePrefab;

    // Idle Variables
    [SerializeField] private float randomMovementRange = 5f;
    [SerializeField] private float randomMovementSpeed = 1f;
    private Vector3 targetPos;

    // Knockback Variables
    [SerializeField] private float knockbackForce;

    public enum EnemyState {
        Idle,
        Chase,
        Attack,
        Death
    }

    private void Start() {
        currentState = EnemyState.Idle;
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        targetPos = GetRandomPointInCircle();
        isFacingRight = true;
        knockbackForce = 5f;
    }

    private void FixedUpdate() {
        if(player != null) {
            if(PlayerDistance().magnitude <= aggroRadius) {
                isAggro = true;
            }
            else {
                isAggro = false;
            }

            if(PlayerDistance().magnitude <= attackRadius) {
                isAttacking = true;
            }
            else {
                isAttacking = false;
            }
        }
        else {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if(player.GetComponent<Animator>().GetBool("isDead")) {
            SetState(EnemyState.Idle);
        }

        switch(currentState) {
            case EnemyState.Idle:
                // Idle Logic
                animator.SetBool("ChaseState", false);
                animator.SetBool("AttackState", false);

                if(isAggro) {
                    SetState(EnemyState.Chase);
                }

                if(collisionHit) {
                    targetPos = GetRandomPointInCircle();
                    collisionHit = false;
                }

                // Wanders around randomly when Idling
                Vector3 direction = (targetPos - transform.position).normalized;
                MoveEnemy(direction * randomMovementSpeed);
                if((transform.position - targetPos).sqrMagnitude < 0.01f) {
                    targetPos = GetRandomPointInCircle();
                }

                break;

            case EnemyState.Chase:
                // Chase Logic
                movementSpeed = 1.75f;
                animator.SetBool("AttackState", false);
                animator.SetBool("ChaseState",true);

                if(!isAggro) {
                    SetState(EnemyState.Idle);
                }

                if(isAttacking) {
                    SetState(EnemyState.Attack);
                }

                // Goes after the player
                Vector2 moveDirection = (player.transform.position - transform.position).normalized;
                MoveEnemy(moveDirection * movementSpeed);
                break;

            case EnemyState.Attack:
                // Attack Logic
                movementSpeed = 2.0f;
                animator.SetBool("ChaseState", false);
                animator.SetBool("AttackState", true);

                if(!isAttacking) {
                    SetState(EnemyState.Chase);
                }

                if(!isRanged) {
                    moveDirection = (player.transform.position - transform.position).normalized;
                    MoveEnemy(moveDirection * movementSpeed);
                    
                    if(playerHit) {
                        Vector2 knockbackDirection = (player.transform.position - transform.position).normalized;
                        player.GetComponent<PlayerMovement>().PlayerKnockback(knockbackDirection);
                        player.GetComponent<PlayerMovement>().TakeDamage(attackDamage); // hits twice???
                        playerHit = false;
                    }
                }
                else {
                    MoveEnemy(Vector2.zero);
                    if(Time.time >= nextAttackTime) {
                        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

                        //Bullet Direction
                        Vector2 shootDirection = (player.transform.position - transform.position).normalized;

                        // Shooting
                        if(projectile != null) {
                            projectile.GetComponent<EnemyProjectile>().Move(shootDirection);
                        }
                        nextAttackTime = Time.time + 5f / attackCooldown;
                    }
                }
                

                break;

            case EnemyState.Death:
                MoveEnemy(Vector2.zero);
                Death();
                break;

        }
    }

    public void SetState(EnemyState newState) {
        currentState = newState;
    }

    #region Movement Functions

    public void MoveEnemy(Vector2 velocity) {
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

    private Vector3 PlayerDistance() {
        return transform.position - player.transform.position;
    }

    #endregion

    #region Idle State Functions

    private Vector3 GetRandomPointInCircle() {
        return transform.position + (Vector3)UnityEngine.Random.insideUnitCircle * randomMovementRange;
    }

    #endregion

    #region Damage and Death

    public void Damage(float damageAmount, Vector2 knockbackDirection) {
        currentHealth -= damageAmount;
        animator.SetTrigger("Damage");
        StartCoroutine(ApplyKnockback(knockbackDirection));

        if(currentHealth <= 0) {
            SetState(EnemyState.Death);
        }
    }

    public void Damage(float damageAmount) {
        currentHealth -= damageAmount;
        animator.SetTrigger("Damage");

        if(currentHealth <= 0) {
            SetState(EnemyState.Death);
        }
    }

    private IEnumerator ApplyKnockback(Vector2 knockbackDirection) {
        float duration = 1f;
        float startTime = Time.time;

        while(Time.time < startTime + duration) {
            rb.velocity = Vector2.zero;
            transform.position += (Vector3)knockbackDirection * (knockbackForce/duration) * Time.deltaTime;
            yield return null;
        }
    }

    public void Death() {
        StartCoroutine(DestroyAfterAnimation());
    }

    private IEnumerator DestroyAfterAnimation() {
        animator.SetBool("DeadState",true);

        // Verifies death animation is playing
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("slime_death")) {
            yield return new WaitForSecondsRealtime(animator.GetCurrentAnimatorStateInfo(0).length);
            Destroy(this.gameObject);
        }
        // if not, wait for current animation to end and recurse Death()
        else {
            yield return new WaitForSecondsRealtime(animator.GetCurrentAnimatorStateInfo(0).length);
            Death();
        }
    }

    #endregion

    #region Collision Checks

    private void OnCollisionStay2D(Collision2D other) {
        if(other.gameObject.CompareTag("Walls") || other.gameObject.CompareTag("Enemy")) {
            collisionHit = true;
        }

        if(other.gameObject.CompareTag("Player")) {
            playerHit = true;
        }
    }

    #endregion

    #region Testing
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, 8f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 3f);
    }
    #endregion

}
