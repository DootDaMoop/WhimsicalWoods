using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 7f;
    [SerializeField] private float projectileDamage = 25f;
    public GameObject enemy;

    private void Start() {
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        projectileSpeed = 7f;
        projectileDamage = 25f;
    }

    public void Move(Vector2 direction) {
        StartCoroutine(FireProjectile(direction));
    }

    private IEnumerator FireProjectile(Vector2 direction) {
        float duration = 2f;
        float startTime = Time.time;

        while(Time.time < startTime + duration) {
            transform.position += (Vector3)direction.normalized * projectileSpeed * Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Walls")) {
            Destroy(gameObject);
        }
        else if(other.gameObject.CompareTag("Enemy")) {
            Debug.Log("HIT");
            other.gameObject.GetComponent<EnemyEnum>().Damage(projectileDamage);
            Destroy(gameObject);
        }
    }
}
