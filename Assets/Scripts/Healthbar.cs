// referenced: https://www.youtube.com/watch?v=FBDN4b9PGgE
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    Slider healthSlider;
    public bool enemy = false;

    // Start is called before the first frame update
    void Start()
    {
        healthSlider = GetComponent<Slider>();
        PlayerPrefs.SetInt("bossHealth", 100);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy) {
            int bossHealth = PlayerPrefs.GetInt("bossHealth");
            healthSlider.value = bossHealth;
        }
        else
        {
            int currentHealth = PlayerPrefs.GetInt("currentHealth");
            healthSlider.value = currentHealth;
        }
    }

    // public void SetMaxHealth(int maxHealth) {
    //     healthSlider.maxHealth = maxHealth;
    //     healthSlider.value = maxHealth;
    // }

    // public void SetHealth(int health) {
    //     healthSlider.value = health;
    // }
}
