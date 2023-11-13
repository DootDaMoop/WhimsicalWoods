using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f; // Adjust the speed as needed
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
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        facingRight = true;
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
        if(rb.velocity == Vector2.zero) {
            AnimateIdle(mouseDirection);
        }
        else {
            AnimateMovement(movement);
        }
        
        // Sprint
        if(Input.GetKey(KeyCode.LeftShift)) {
            moveSpeed = 15f;
        }
        else {
            moveSpeed = 5f;
        }
    }

    #region Player Movement
    public void AnimateMovement(Vector2 movement) {
        animator.SetBool("isMoving", true);
        animator.SetFloat("inputX",movement.x);
        animator.SetFloat("inputY",movement.y);
        if((movement.x > 0) && !facingRight) {
            FlipOnX();
        }
        if((movement.x < 0) && facingRight) {
            FlipOnX();
        }
    }

    public void AnimateIdle(Vector2 direction) {
        animator.SetBool("isMoving",false);
        animator.SetFloat("inputX",direction.x);
        animator.SetFloat("inputY",direction.y); 
        if((mouseDirection.x > 0) && !facingRight) {
            FlipOnX();
        }
        if((mouseDirection.x < 0) && facingRight) {
            FlipOnX();
        }
    }
    
    public void FlipOnX() {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        facingRight = !facingRight;
    }

    #endregion

    /*private void OnDrawGizmosSelected() {
        if (transform == null) {
            //Debug.Log("Attack Point is null!");
            return;
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + (Vector3)mouseDirection, 1f);
    }*/
}
