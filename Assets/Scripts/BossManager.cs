using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    // Start is called before the first frame update
    void Start()
    {
        // Instantiate the player after the dungeon has been generated
        Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
