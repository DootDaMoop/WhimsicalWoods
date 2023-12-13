using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatCapybara : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackRange;
    [SerializeField] private float comboCooldown = 1f;
    [SerializeField] private float comboResetTime = 1f;
    private int comboCount = 0;
    private float lastAttackTime = 0f;
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
        if(Input.GetMouseButton(0)) {
            StartCoroutine(PlayerCombo());
        }

        // Check if the player hasn't attacked for a while, reset the combo
        if (Time.time - lastAttackTime >= comboResetTime) {
            //Debug.Log("Combo Reset");
            comboCount = 0;
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
                if(enemy.GetComponent<EnemyEnum>() != null) {
                    enemy.GetComponent<EnemyEnum>().Damage(attackDamage, knockbackDirection);
                }
                else { 
                    enemy.GetComponent<BossAI>().Damage(attackDamage, knockbackDirection);
                }
                
            }
        }
    }

    private IEnumerator PlayerCombo() {
        if(Time.time >= lockedMovementTime) {
            lockedMovementTime = Time.time + 0.5f;

            if(comboCount < 3) {
                PlayerAttack();
                comboCount++;
                lastAttackTime = Time.time;
                Debug.Log($"Combo Mark: {comboCount}");
            }
            else {
                Debug.Log("Combo Break");
                yield return new WaitForSecondsRealtime(comboCooldown);
                comboCount = 0;
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
