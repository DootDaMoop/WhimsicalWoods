using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    int currentHealth = 100;
    // Start is called before the first frame update
    void Start()
    {
        int currentHealth = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0) {
            SceneManager.LoadScene("Lose", LoadSceneMode.Single);
        }
        // testing hb ui
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            int currentHealth = PlayerPrefs.GetInt("currentHealth");
            currentHealth -= 10;
            PlayerPrefs.SetInt("currentHealth", currentHealth);
            // TakeDamage(10);
            Debug.Log("Current Health: " + currentHealth);
        }
        else if (Input.GetKeyDown(KeyCode.Equals))
        {
            int currentHealth = PlayerPrefs.GetInt("currentHealth");
            currentHealth += 10;
            PlayerPrefs.SetInt("currentHealth", currentHealth);
            // TakeDamage(-10);
            Debug.Log("Current Health: " + currentHealth);
        }
    }
}
