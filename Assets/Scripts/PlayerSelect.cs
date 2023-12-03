using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelect : MonoBehaviour
{
    public GameObject[] characters;

    public int selectedCharacter = 0;

    void Start()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            if (i > 0)
            {
                characters[i].SetActive(false);
            }
        }
    }



    public void NextCharacter()
    {
        characters[selectedCharacter].SetActive(false);
        selectedCharacter = (selectedCharacter + 1) % characters.Length;
        characters[selectedCharacter].SetActive(true);
    }

    public void PreviousCharacter()
    {
        characters[selectedCharacter].SetActive(false);
        selectedCharacter--;
        if (selectedCharacter < 0)
        {
            selectedCharacter += characters.Length;
        }
        characters[selectedCharacter].SetActive(true);
    }

    public void StartGame()
    {
        PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
        // string sceneName = "Game";
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
        // SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
