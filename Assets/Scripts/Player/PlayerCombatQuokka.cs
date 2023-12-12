using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatQuokka : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackCooldown = 1f; // Adjust as needed
    [SerializeField] private float attackSpeed = 10f;
    private float nextAttackTime = 0f;
    private float lockedMovementTime = 0f;
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
            //lockedMovementTime = Time.time + 0.5f;
            PlayerShoot();
            nextAttackTime = Time.time + 1f / attackCooldown;
        }
        
    }

    void PlayerShoot() {
        GameObject projectile = Instantiate(projectilePrefab, attackPoint.position, Quaternion.identity);

        // Bullet Direction
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 shootDirection = (mousePosition - transform.position).normalized;

        Debug.DrawRay(attackPoint.position, shootDirection * 10f, Color.green, 2f);

        // Shooting
        if(projectile != null) {
            projectile.GetComponent<Projectile>().Move(shootDirection);
        }
    }

    private void OnDrawGizmosSelected() {
        if (transform == null) {
            //Debug.Log("Attack Point is null!");
            return;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, 1f);
    }
    
}
