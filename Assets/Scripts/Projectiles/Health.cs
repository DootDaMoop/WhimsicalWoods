using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float healAmount= 10f;
    [SerializeField] private GameObject player;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player")) {
            player.GetComponent<PlayerMovement>().TakeDamage(-healAmount);
            Destroy(gameObject);
        }
    }
}
