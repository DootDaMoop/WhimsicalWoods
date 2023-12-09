// referenced: https://www.youtube.com/watch?v=FBDN4b9PGgE
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    Slider healthSlider;

    // Start is called before the first frame update
    void Start()
    {
        healthSlider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        int currentHealth = PlayerPrefs.GetInt("currentHealth");
        healthSlider.value = currentHealth;
    }

    // public void SetMaxHealth(int maxHealth) {
    //     healthSlider.maxHealth = maxHealth;
    //     healthSlider.value = maxHealth;
    // }

    // public void SetHealth(int health) {
    //     healthSlider.value = health;
    // }
}
