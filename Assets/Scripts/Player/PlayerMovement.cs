using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Adjust the speed as needed
    private Rigidbody2D rb;
    private Animator animator;
    public GameObject exitPrefab;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Get input from the player
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // Calculate movement direction
        Vector2 movement = new Vector2(moveX, moveY).normalized;

        // Apply movement
        rb.velocity = movement * moveSpeed;

        #region Running Animations

        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            animator.SetBool("MovingHorizontal",true);
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            animator.SetBool("MovingHorizontal",true);
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
        }
        else {
            animator.SetBool("MovingHorizontal",false);
        }

        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            animator.SetBool("MovingUp",true);
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            animator.SetBool("MovingDown",true);
        }
        else {
            animator.SetBool("MovingUp",false);
            animator.SetBool("MovingDown",false);
        }

        #endregion

        // Sprint
        if(Input.GetKey(KeyCode.LeftShift)) {
            moveSpeed = 15f;
        }
        else {
            moveSpeed = 5f;
        }
    }
}
