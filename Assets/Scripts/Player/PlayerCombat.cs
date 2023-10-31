using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private Animator animator {get; set;}
    [field: SerializeField] public GameObject player {get; set;}
    [field: SerializeField] public float maxHealth {get; set;}
    public float currentHealth {get; set;}
    [field: SerializeField] public float attackDamage {get; set;} = 50f;
    [field: SerializeField] public Transform attackPoint {get; set;}
    [field: SerializeField] public float attackRange {get; set;} = 0.75f;
    [field: SerializeField] public float attackRate {get; set;} = 2f;
    [field: SerializeField] public float nextAttackTime {get; set;} = 0f;
    [field: SerializeField] public float knockbackForce {get; set;} = 0.01f;
    [field: SerializeField] public GameObject enemy {get; set;}
    [field: SerializeField] public LayerMask enemyLayers {get; set;}
    public bool hitByEnemy {get; set;} = false;
    public bool stayDead {get; set;} = false;


    private void Start() {
        animator = GetComponent<Animator>();
        attackPoint = GameObject.FindGameObjectWithTag("AttackPoint").transform;
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        currentHealth = maxHealth;
    }

    private void FixedUpdate() {
        if(attackPoint == null) {
            attackPoint = GameObject.FindGameObjectWithTag("AttackPoint").transform;
        }
        if(enemy == null) {
            enemy = GameObject.FindGameObjectWithTag("Enemy");
        }

        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            //Right;
            attackPoint.position = player.transform.position + new Vector3(0.5f,-0.35f);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            //Left;
            attackPoint.position = player.transform.position + new Vector3(-0.5f,-0.35f);
        }

        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            //Up;
            attackPoint.position = player.transform.position;
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            //Down;
            attackPoint.position = player.transform.position + new Vector3(0f,-1f);
        }

        if(Time.time >= nextAttackTime && !stayDead) {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButton(0)) {
                PlayerAttack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }

        if(hitByEnemy && !stayDead && enemy != null) {
            Vector2 direction = (transform.position - enemy.transform.position).normalized;
            Vector2 knockback = direction * knockbackForce;
            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<Rigidbody2D>().AddForce(knockback,ForceMode2D.Impulse);
            GetComponent<PlayerMovement>().enabled = true;
            hitByEnemy = false;
            TakeDamage(20);
            Debug.Log("Current Health: "+currentHealth);
            
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
                //Debug.Log("Hit!");
                enemy.GetComponent<Enemy>().Damage(attackDamage);
                Vector2 direction = (enemy.GetComponent<Enemy>().transform.position - attackPoint.position).normalized;
                Vector2 knockback = direction * knockbackForce;
                enemy.GetComponent<Enemy>().rb.AddForce(knockback, ForceMode2D.Impulse);
            }
        }
    }

    private void OnDrawGizmosSelected() {
        if (attackPoint == null) {
            //Debug.Log("Attack Point is null!");
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private void TakeDamage(float damageAmount) {
        currentHealth -= damageAmount;

        if(currentHealth <= 0) {
            PlayerDeath();
        }
    }

    private void PlayerDeath() {
        animator.SetBool("PlayerDied", true);
        stayDead = true;


        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        this.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag.Equals("Enemy")) {
            hitByEnemy = true;
        }
    }
}
