using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private float cameraSpeed;
    private GameObject target;

    void Start() {
        target = GameObject.FindWithTag("Player");
    }

    void FixedUpdate() {
        // Checks if target was initalized
        if(target == null) {
            target = GameObject.FindWithTag("Player");
        }
        else{
            // Follows target (Player)
            Vector3 pos = new Vector3(target.transform.position.x,target.transform.position.y,-10f);
            transform.position = Vector3.Slerp(transform.position,pos,cameraSpeed*Time.deltaTime);  
        }

        if(Input.GetKey(KeyCode.LeftShift)) {
            cameraSpeed = 5f;
        }
        else {
            cameraSpeed = 2f;
        }
        
    }
}
