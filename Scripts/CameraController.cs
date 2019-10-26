using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour{

    public GameObject player; //Assign the GameObject in the hierarchy panel
    private Vector3 offset; 

    // Start is called before the first frame update
    void Start(){
        offset = transform.position - player.transform.position; //Return position of GameObject Player
        Debug.Log(offset);
    }

    // Update is called once per frame
    void Update(){
        
    }

    //Exec once per frame but after of method Update()
    void LateUpdate(){
        transform.position = player.transform.position + offset; //Restart the position for the camera to distance calculate on Start()
    }
}
