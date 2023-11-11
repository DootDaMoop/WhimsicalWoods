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
    private Vector2 movement;
    private bool facingRight;
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

        // Calculate movement direction
        movement = new Vector2(moveX, moveY).normalized;

        // Apply movement
        rb.velocity = movement * moveSpeed;

        // Animations
        AnimateMovement(movement);
        if(movement.x > 0 && !facingRight) {
            FlipOnX();
        }
        if(movement.x < 0 && facingRight) {
            FlipOnX();
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
        if(rb.velocity != Vector2.zero) {
            animator.SetBool("isMoving", true);
            animator.SetFloat("inputX",movement.x);
            animator.SetFloat("inputY",movement.y);
        }
        else {
            animator.SetBool("isMoving", false);
        }
    }
    
    public void FlipOnX() {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        facingRight = !facingRight;
    }
    #endregion
}
