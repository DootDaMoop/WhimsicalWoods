using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatCapybara : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCooldown = 1f; // Adjust as needed
    private float nextAttackTime = 0f;
    private float lockedMovementTime = 0f;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private GameObject enemy;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector3 mousePosition;
    private Vector2 mouseDirection;

    private void Start() {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        // Sets up Attack Point Position
        mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        mouseDirection = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y).normalized;
        attackPoint.position = transform.position + (Vector3)mouseDirection;

        //Debug.Log(Time.time >= nextAttackTime);

        if(!(Time.time >= lockedMovementTime)) {
            // Note: Left Does Not Work Due to Lack of Left Swing Animation (For Now)
            animator.SetFloat("inputX",mouseDirection.x);        
            animator.SetFloat("inputY",mouseDirection.y);
            rb.velocity = Vector2.zero;
            animator.SetBool("isMoving",false);
        }

        // Attacking
        if(Input.GetMouseButton(0) && Time.time >= nextAttackTime) {
            lockedMovementTime = Time.time + 0.5f;

            PlayerAttack();
            nextAttackTime = Time.time + 1f / attackCooldown;
        }
        
    }

    void PlayerAttack() {
        animator.SetTrigger("Attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies) {
            if(hitEnemies.Length == 0) {
                return;
            }
            else{
                Debug.Log("Hit!");
                Vector2 knockbackDirection = CalculateKnockbackDirection(enemy);
                enemy.GetComponent<EnemyEnum>().Damage(attackDamage, knockbackDirection);
            }
        }
    }

    private Vector2 CalculateKnockbackDirection(Collider2D enemyCollider) {
        return (enemyCollider.transform.position - transform.position).normalized;
    }

    private void OnDrawGizmosSelected() {
        if (transform == null) {
            //Debug.Log("Attack Point is null!");
            return;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
