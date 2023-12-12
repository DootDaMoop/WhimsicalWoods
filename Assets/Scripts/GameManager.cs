using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    GameObject forest;
    GameObject arctic;
    GameObject jungle;

    public int currentHealth = 100;

    // Start is called before the first frame update
    void Start()
    {
        forest = GameObject.Find("ForestDungeon");
        arctic = GameObject.Find("ArcticDungeon");
        jungle = GameObject.Find("JungleDungeon");

        forest.SetActive(true);
        arctic.SetActive(false);
        jungle.SetActive(false);

        PlayerPrefs.SetInt("currentHealth", currentHealth);
        PlayerPrefs.SetInt("boss", 0);
        
    }

    // Update is called once per frame
    void Update()
    {
        // // testing hb ui
        // if (Input.GetKeyDown(KeyCode.Minus))
        // {
        //     int currentHealth = PlayerPrefs.GetInt("currentHealth");
        //     currentHealth -= 10;
        //     PlayerPrefs.SetInt("currentHealth", currentHealth);
        //     // TakeDamage(10);
        //     Debug.Log("Current Health: " + currentHealth);
        // }
        // else if (Input.GetKeyDown(KeyCode.Equals))
        // {
        //     int currentHealth = PlayerPrefs.GetInt("currentHealth");
        //     currentHealth += 10;
        //     PlayerPrefs.SetInt("currentHealth", currentHealth);
        //     // TakeDamage(-10);
        //     Debug.Log("Current Health: " + currentHealth);
        // }
        if (PlayerPrefs.GetInt("currentHealth") <= 0) {
            SceneManager.LoadScene("Lose", LoadSceneMode.Single);
        }

        // if (forest.activeSelf == false) {
        //     if (forest.biomeComplete == true) {
        //     Debug.Log("forest biome complete");
        //     forest.SetActive(false);
        //     arctic.SetActive(true);
        //     jungle.SetActive(false);
        //     RoomFirstDungeonGenerator.biomeComplete = false;
        //     }
        // }
        if (RoomFirstDungeonGenerator.biomeComplete == true) {
            if (forest.activeSelf) {
                Debug.Log("forest biome complete");
                forest.SetActive(false);
                arctic.SetActive(true);
                jungle.SetActive(false);
                RoomFirstDungeonGenerator.biomeComplete = false;
                PlayerPrefs.SetInt("currentHealth", currentHealth);
            } else if (arctic.activeSelf) {
                Debug.Log("arctic biome complete");
                forest.SetActive(false);
                arctic.SetActive(false);
                jungle.SetActive(true);
                RoomFirstDungeonGenerator.biomeComplete = false;
                PlayerPrefs.SetInt("currentHealth", currentHealth);
            } else if (jungle.activeSelf) {
                Debug.Log("jungle biome complete");
                PlayerPrefs.SetInt("boss", 1);
                PlayerPrefs.SetInt("currentHealth", currentHealth);
                SceneManager.LoadScene("Boss", LoadSceneMode.Single);
            }
                
        }
        // if (arctic.activeSelf && RoomFirstDungeonGenerator.biomeComplete == true){
        //     Debug.Log("arctic biome complete");
        //     forest.SetActive(false);
        //     arctic.SetActive(false);
        //     jungle.SetActive(true);
        //     RoomFirstDungeonGenerator.biomeComplete = false;
        // }
    }
}
