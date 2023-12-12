using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float projectileDamage = 10f;
    public GameObject player;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Enemy");
        projectileSpeed = 10f;
        projectileDamage = 10f;
    }

    public void Move(Vector2 direction) {
        StartCoroutine(FireProjectile(direction));
    }

    private IEnumerator FireProjectile(Vector2 direction) {
        float duration = 2f;
        float startTime = Time.time;

        while(Time.time < startTime + duration) {
            transform.position += (Vector3)direction * (projectileSpeed/duration) * Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Walls")) {
            Destroy(gameObject);
        }
        else if(other.gameObject.CompareTag("Player")) {
            other.gameObject.GetComponent<PlayerMovement>().TakeDamage(projectileDamage);
            Destroy(gameObject);
        }
    }
}
