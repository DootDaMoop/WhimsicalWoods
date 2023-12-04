using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // This line imports the TextMeshPro namespace

public class PlayerSelect : MonoBehaviour
{
    public GameObject[] characters;
    public string[] names;
    public TMP_Text label;

    public int selectedCharacter = 0;

    void Start()
    {
        label.text = names[0];
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
        label.text = names[selectedCharacter];
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
        label.text = names[selectedCharacter];
    }

    public void StartGame()
    {
        PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
        // string sceneName = "Game";
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
        // SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}

