using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossManager : MonoBehaviour
{
    public GameObject[] characterPrefabs;
    public GameObject enemyPrefab;
    // public int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate the player after the dungeon has been generated
        int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter");
        GameObject playerPrefab = characterPrefabs[selectedCharacter];
        Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

        int currentHealth = PlayerPrefs.GetInt("currentHealth");
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("currentHealth") <= 0) {
            SceneManager.LoadScene("Lose", LoadSceneMode.Single);
        }
        if (PlayerPrefs.GetInt("bossHealth") <= 0) {
            SceneManager.LoadScene("Win", LoadSceneMode.Single);
        }
    }
}