using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BallController : MonoBehaviour{
    
    private Rigidbody rb;
    
    public float speed = 5; //The sentence public show in the inspector this variable
    public Transform particles;
    private ParticleSystem systemParticles;
    private Vector3 position;
    private int cubos;
    private AudioSource audioRecolectable;

    public Text textCounter;
    public Text textCongratulation;
    
    
    // Start is called before the first frame update
    void Start(){
        systemParticles = particles.GetComponent<ParticleSystem>();
        systemParticles.Stop();

        rb = GetComponent<Rigidbody>();                                          //Get components of game object that contain this script

        audioRecolectable = GetComponent<AudioSource>();

        cubos = GameObject.FindGameObjectsWithTag("Recolectable").Length;        //Obtain a number of object with a tag

        textCounter.text = "Contador: " + cubos.ToString();

        transform.localScale = new Vector3(3,3,3);
        
    }

    // Update is called once per frame
    void Update(){
        
    }

    //Exec once each frame after of physic calculates 
    void FixedUpdate(){
        float moveHorizontal = Input.GetAxis("Horizontal");                      //Input.GetAxis return -1 to 1  
        float moveVertical = Input.GetAxis("Vertical");

 
        //Vector3 is a array of size 3 X,Y,Z 
        Vector3 move = new Vector3(moveHorizontal, 0.0f, moveVertical);
        transform.Rotate (new Vector3(0,0,180)*Time.deltaTime); //Rotation effect to object  deltatime = seconds

        //Apply movement to component Rigidbody
        rb.AddForce(move*speed);
    }

    //This a automatic function that listen a event of collation between anything object
    void OnTriggerEnter(Collider otherObject){                                  // "other" is a reference of collider that touch this object
        if(otherObject.gameObject.CompareTag("Finish")){
            transform.localScale = new Vector3(3,1,3);

            position = otherObject.gameObject.transform.position;
            particles.position = position;
            systemParticles = particles.GetComponent<ParticleSystem>();
            systemParticles.Play();
            
            //Destroy(otherObject.gameObject);                                      //Destroy eliminated the object game of memory  
            otherObject.gameObject.SetActive(false);                                // Object is recolactable

            audioRecolectable.Play();                                               //Play a sound of gameObject  

            cubos--;
            
            textCounter.text = "Contador: " + cubos.ToString();

            if(cubos <= 0){
                Debug.Log("Level Up!!");
                //SceneManager.LoadScene(1);                                          //Change a other Scene  >Number of scene (File>BuildSttings) 
                textCongratulation.text = "Level Up !!!";
            }


        }else{
                                                                                //Object NOT is recolectable
        }
    }
}
