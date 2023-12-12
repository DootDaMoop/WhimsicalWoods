using System.Collections;
using UnityEngine;

using UnityEngine.SceneManagement;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f; // Adjust the speed as needed
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    [SerializeField] private float knockbackForce;
    public GameObject enemy;
    private bool isHit;
    private Rigidbody2D rb;
    private Animator animator;
    private float moveX;
    private float moveY;
    private Vector3 mousePosition;
    private Vector2 movement;
    private Vector2 mouseDirection;
    private bool facingRight;
    [SerializeField] private GameObject swordHitbox;
    public GameObject exitPrefab;

    private void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        isHit = false;
        currentHealth = PlayerPrefs.GetInt("currentHealth");
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        facingRight = true;
        knockbackForce = 5f;
    }

    private void FixedUpdate()
    {
        // Player Movement Input
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
        mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Calculate directions
        movement = new Vector2(moveX, moveY).normalized;
        mouseDirection = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y).normalized;

        // Apply movement
        rb.velocity = movement * moveSpeed;


        // Animations
        if (rb.velocity == Vector2.zero) {
            AnimateIdle(mouseDirection);
        }
        else {
            AnimateMovement(movement);
        }

        // Sprint
        if (Input.GetKey(KeyCode.LeftShift)) {
            moveSpeed = 15f;
        }
        else {
            moveSpeed = 5f;
        }
    }

    #region Player Movement
    public void AnimateMovement(Vector2 movement)
    {
        animator.SetBool("isMoving", true);
        animator.SetFloat("inputX", movement.x);
        animator.SetFloat("inputY", movement.y);
        if ((movement.x > 0) && !facingRight)
        {
            FlipOnX();
        }
        if ((movement.x < 0) && facingRight)
        {
            FlipOnX();
        }
    }

    public void AnimateIdle(Vector2 direction)
    {
        animator.SetBool("isMoving", false);
        animator.SetFloat("inputX", direction.x);
        animator.SetFloat("inputY", direction.y);
        if ((mouseDirection.x > 0) && !facingRight)
        {
            FlipOnX();
        }
        if ((mouseDirection.x < 0) && facingRight)
        {
            FlipOnX();
        }
    }

    public void FlipOnX()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        facingRight = !facingRight;
    }

    #endregion

    #region Player Damage

    public void TakeDamage(float damageAmount) {
        isHit = false;
        // currentHealth -= damageAmount;
        currentHealth = PlayerPrefs.GetInt("currentHealth");
        currentHealth -= (int)damageAmount;
        PlayerPrefs.SetInt("currentHealth", currentHealth);
        Debug.Log($"Current Health: {currentHealth}");

        if(currentHealth <= 0) {
            PlayerDeath();
            SceneManager.LoadScene("Lose", LoadSceneMode.Single);
        }
    }

    public void PlayerKnockback(Vector2 knockbackDirection) {
        StartCoroutine(KnockbackPlayer(knockbackDirection));
    }

    private IEnumerator KnockbackPlayer(Vector2 direction) {
        float duration = 0.5f;
        float startTime = Time.time;

        while(Time.time < startTime + duration) {
            transform.position += (Vector3)direction * (knockbackForce / duration) * Time.deltaTime;
            enemy.GetComponent<EnemyEnum>().rb.velocity = Vector2.zero;
            yield return null;
        }
    }

    private void PlayerDeath() {
        animator.SetBool("isDead", true);
        StartCoroutine(DestroyAfterAnimation());
    }

    IEnumerator DestroyAfterAnimation() {
        animator.SetBool("isDead",true);

        // Verifies death animation is playing
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("character_death")) {
            yield return new WaitForSecondsRealtime(animator.GetCurrentAnimatorStateInfo(0).length);
            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            if(GetComponent<PlayerCombatCapybara>() != null) {
                GetComponent<PlayerCombatCapybara>().enabled = false;
            }
            else if(GetComponent<PlayerCombatQuokka>() != null) {
                GetComponent<PlayerCombatQuokka>().enabled = false;
            }
        }
        // if not, wait for current animation to end and recurse Death()
        else {
            yield return new WaitForSecondsRealtime(animator.GetCurrentAnimatorStateInfo(0).length);
            PlayerDeath();
        }
    }

    #endregion

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Enemy")) {
            isHit = true;
        }
    }

    /*private void OnDrawGizmosSelected() {
        if (transform == null) {
            //Debug.Log("Attack Point is null!");
            return;
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + (Vector3)mouseDirection, 1f);
    }*/
}
