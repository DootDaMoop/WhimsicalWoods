using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackRadiusCheck : MonoBehaviour
{
    public GameObject player {get; set;}
    private Enemy enemy;

    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject == player) {
            enemy.setIsWithinAttackRadius(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject == player) {
            enemy.setIsWithinAttackRadius(false);
        }
    }
}
