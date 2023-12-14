using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    // Universal Variables
    private EnemyState currentState;
    [SerializeField] private float maxHealth = 100f;
    public int currentHealth;
    [SerializeField] private float attackDamage = 10f;
    public Rigidbody2D rb;
    public Animator animator;
    public GameObject player;
    public bool collisionHit {get; set;} =false; // Collision with wall
    public bool playerHit {get; set;} = false; // Collision with Player
    public bool isAggro {get; set;}
    public bool isAttacking {get; set;}
    public float aggroRadius = 10f;
    public float attackRadius = 9f;
    public float movementSpeed = 1.75f;
    public bool coroutineRunning {get; set;} = false;
    [SerializeField] private float nextAttackTime = 0f;
    [SerializeField] private float attackCooldown = 1f;
    public GameObject projectilePrefab;
    public SpriteRenderer spriteRenderer;
    public PolygonCollider2D polygonCollider;
    [SerializeField] private AudioSource attackSoundEffect;
    [SerializeField] private AudioSource meleeSoundEffect;
    [SerializeField] private AudioSource rangedSoundEffect;


    // Knockback Variables
    [SerializeField] private float knockbackForce;

    public enum EnemyState {
        Chase,
        Attack,
        Death
    }

    private void Start() {
        currentState = EnemyState.Chase;
        currentHealth = PlayerPrefs.GetInt("bossHealth");
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        polygonCollider = GetComponent<PolygonCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        knockbackForce = 5f;
    }

    private void FixedUpdate() {
        // Movement Animations
        float xInput = rb.velocity.x;
        float yInput = rb.velocity.y;

        animator.SetFloat("MoveX", xInput);
        animator.SetFloat("MoveY", yInput);

        Vector2[] spriteVertices = spriteRenderer.sprite.vertices;
        polygonCollider.SetPath(0, spriteVertices);

        if(player != null) {
            if(PlayerDistance().magnitude <= attackRadius) {
                isAttacking = true;
                attackSoundEffect.Play();
            }
            else {
                isAttacking = false;
            }
        }
        else {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if(player.GetComponent<Animator>().GetBool("isDead")) {
            SetState(EnemyState.Chase);
        }

        switch(currentState) {

            case EnemyState.Chase:
                // Chase Logic
                animator.speed = 0.25f;
                movementSpeed = 1.75f;
                //animator.SetBool("AttackState", false);
                //animator.SetBool("ChaseState",true);

                if(isAttacking) {
                    SetState(EnemyState.Attack);
                }

                // Goes after the player
                Vector2 moveDirection = (player.transform.position - transform.position).normalized;
                MoveEnemy(moveDirection * movementSpeed);
                break;

            case EnemyState.Attack:
                // Attack Logic
                animator.speed = 0.5f;
                movementSpeed = 2.0f;
                //animator.SetBool("ChaseState", false);
                //animator.SetBool("AttackState", true);

                if(playerHit) {
                    Vector2 knockbackDirection = (player.transform.position - transform.position).normalized;
                    player.GetComponent<PlayerMovement>().PlayerKnockback(knockbackDirection);
                    player.GetComponent<PlayerMovement>().TakeDamage(attackDamage); 
                    playerHit = false;
                }

                if(!isAttacking) {
                    SetState(EnemyState.Chase);
                }

                moveDirection = (player.transform.position - transform.position).normalized;
                MoveEnemy(moveDirection * movementSpeed);
                

                if(animator.GetBool("Damaged")) {
                    animator.speed = 1f;
                }
                else{
                    animator.speed = 0f;
                }
                
                if(Time.time >= nextAttackTime) {
                    // Projectile Direction
                    Vector2 shootDirection = (player.transform.position - transform.position).normalized;

                    // Projectile Creation
                    GameObject projectile = Instantiate(projectilePrefab, transform.position + (Vector3)shootDirection, Quaternion.identity);

                    // Shooting
                    if(projectile != null) {
                        projectile.GetComponent<EnemyProjectile>().Move(shootDirection);
                    }
                    nextAttackTime = Time.time + 0.8f;
                }
                break;

            case EnemyState.Death:
                animator.speed = 1f;
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
        //CheckForLeftOrRightFacing(velocity);
    }

    /*public void CheckForLeftOrRightFacing(Vector2 velocity)
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
    }*/

    private Vector3 PlayerDistance() {
        return transform.position - player.transform.position;
    }

    #endregion

    #region Damage and Death

    public void Damage(float damageAmount, Vector2 knockbackDirection) {
        currentHealth = PlayerPrefs.GetInt("bossHealth");
        currentHealth -= (int) damageAmount;
        meleeSoundEffect.Play();
        PlayerPrefs.SetInt("bossHealth", currentHealth);
        animator.SetBool("Damaged", true);
        StartCoroutine(ApplyKnockback(knockbackDirection));

        if(currentHealth <= 0) {
            animator.SetBool("Damaged", false);
            SetState(EnemyState.Death);
        }
    }

    public void Damage(float damageAmount) {
        currentHealth = PlayerPrefs.GetInt("bossHealth");
        currentHealth -= (int) damageAmount;
        rangedSoundEffect.Play();
        PlayerPrefs.SetInt("bossHealth", currentHealth);
        animator.SetBool("Damaged", true);

        if(currentHealth <= 0) {
            animator.SetBool("Damaged", false);
            SetState(EnemyState.Death);
        }
    }

    private IEnumerator ApplyKnockback(Vector2 knockbackDirection) {
        coroutineRunning = true;
        float duration = 1f;
        float startTime = Time.time;

        while(Time.time < startTime + duration) {
            rb.velocity = Vector2.zero;
            transform.position += (Vector3)knockbackDirection * (knockbackForce/duration) * Time.deltaTime;
            yield return null;
        }

        animator.SetBool("Damaged", false);
        coroutineRunning = false;
    }

    public void Death() {
        StartCoroutine(DestroyAfterAnimation());
    }

    private IEnumerator DestroyAfterAnimation() {
        animator.SetBool("DeadState",true);

        // Verifies death animation is playing
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("death_animation")) {
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
        Gizmos.DrawSphere(transform.position, aggroRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, attackRadius);
    }
    #endregion
}
