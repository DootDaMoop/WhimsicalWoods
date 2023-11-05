using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    GameObject forest;
    GameObject arctic;
    GameObject jungle;

    // Start is called before the first frame update
    void Start()
    {
        forest = GameObject.Find("ForestDungeon");
        arctic = GameObject.Find("ArcticDungeon");
        jungle = GameObject.Find("JungleDungeon");

        forest.SetActive(true);
        arctic.SetActive(false);
        jungle.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
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
            } else if (arctic.activeSelf) {
                Debug.Log("arctic biome complete");
                forest.SetActive(false);
                arctic.SetActive(false);
                jungle.SetActive(true);
                RoomFirstDungeonGenerator.biomeComplete = false;
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
