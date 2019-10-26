    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    public class PlayerController : MonoBehaviour
    {
        public GameObject cubeMovile,cuboDissapear,cubeMulticolor;
        private Rigidbody rb;
        public float speed = 5; //The sentence public show in the inspector this variable
        public Transform particles;
        private ParticleSystem systemParticles;
        private Vector3 position;
        private int cubos,rutina,color;
        private AudioSource audioRecolectable;
        public Text textCounter,textCongratulation;
        public Material mat;
        Animator animator;
        public GameObject power;
        public ParticleSystem floorPower;
        public ParticleSystem roundPower;



        // Start is called before the first frame update
        void Start()
        {
            systemParticles = particles.GetComponent<ParticleSystem>();
            systemParticles.Stop();

            rb = GetComponent<Rigidbody>();                                          //Get components of game object that contain this script

            audioRecolectable = GetComponent<AudioSource>();

            cubos = GameObject.FindGameObjectsWithTag("Recolectable").Length;        //Obtain a number of object with a tag

            textCounter.text = "Contador: " + cubos.ToString();
            color = 0;
            rutina = 0;

            animator = GetComponent<Animator>();

            StartCoroutine(movement());
            StartCoroutine(dissapear());
            StartCoroutine(multicolorCubeShow(color));

        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetButtonDown("Fire1")){ //Detect if button is down
                Animate();
            }

            if(Input.GetButtonDown("Fire2")){ //Detect if button is touch(Up, Down)
                SendPower("Fire2");
            }

            if(Input.GetButtonDown("Fire3")){ //Detect if button is touch(Up, Down)
                SendPower("Fire3");
                
            }
        }

        //Exec once each frame after of physic calculates 
        void FixedUpdate()
        {
            float moveHorizontal = Input.GetAxis("Horizontal");                      //Input.GetAxis return -1 to 1  
            float moveVertical = Input.GetAxis("Vertical");

            //Vector3 is a array of size 3 X,Y,Z 
            Vector3 move = new Vector3(moveHorizontal, 0.0f, moveVertical);

            //Apply movement to component Rigidbody
            rb.AddForce(move * speed);


        }

        //This a automatic function that listen a event of collation between anything object
        void OnTriggerEnter(Collider otherObject)
        {                                  // "other" is a reference of collider that touch this object
            try
            {
                if (otherObject.gameObject.CompareTag("Recolectable"))
                {

                    position = otherObject.gameObject.transform.position;
                    particles.position = position;
                    systemParticles = particles.GetComponent<ParticleSystem>();
                    systemParticles.Play();

                    //Destroy(otherObject.gameObject);                                      //Destroy eliminated the object game of memory  
                    otherObject.gameObject.SetActive(false);                                // Object is recolactable

                    audioRecolectable.Play();                                               //Play a sound of gameObject  

                    cubos--;

                    //textCounter.text = "Contador: " + cubos.ToString();

                    if (cubos <= 0)
                    {
                        Debug.Log("Level Up!!");
                        //SceneManager.LoadScene(1);                                          //Change a other Scene  >Number of scene (File>BuildSttings) 
                        textCongratulation.text = "Level Up !!!";
                    }


                }
                else if (otherObject.gameObject.CompareTag("Multicolor"))
                {
                    rutina = 1;
                    otherObject.gameObject.SetActive(false);
                    audioRecolectable.Play();
                    StopCoroutine("multicolorCubeHide");
                    StopCoroutine("multicolorCubeShow");

                }
            }
            catch (System.Exception)
            {

                throw;
            }

        }

        public IEnumerator movement()
        {
            for (; ; )
            {
                if (Vector3.Distance(transform.position, cubeMovile.transform.position) < 6f)
                {
                    cubeMovile.transform.position = Vector3.Lerp(cubeMovile.transform.position,
                                                    new Vector3(Random.Range(-10.0f, 10.0f), 0.97f, Random.Range(-10.0f, 10.0f)),
                                                    Time.deltaTime);
                }

                yield return new WaitForSecondsRealtime(0.1f);
            }
        }

        public IEnumerator dissapear()
        {
            yield return new WaitForSecondsRealtime(10);
            cuboDissapear.SetActive(false);
        }

        public IEnumerator multicolorCubeHide()
        {
            cubeMulticolor.SetActive(false);
            yield return new WaitForSecondsRealtime(5f);
            if (color == 4) { color = 0; } else { color = color + 1; }
            StartCoroutine(multicolorCubeShow(color));

        }

        public IEnumerator multicolorCubeShow(int color)
        {
            if (rutina == 0)
            {
                switch (color)
                {
                    case 0:
                        mat.color = Color.green;
                        break;
                    case 1:
                        mat.color = Color.black;
                        break;
                    case 2:
                        mat.color = Color.red;
                        break;
                    case 3:
                        mat.color = Color.white;
                        break;
                    case 4:
                        mat.color = Color.blue;
                        break;
                }

                cubeMulticolor.SetActive(true);

                yield return new WaitForSecondsRealtime(3f);
                StartCoroutine(multicolorCubeHide());
            }
        }

        public void SendPower(string key){
            if(key == "Fire2"){
                Debug.Log("Fire2");
                StartCoroutine(sendPowerCoroutine());
            }

            if(key == "Fire3"){
                Debug.Log("Fire3");
                StartCoroutine(elevatePowerCoroutine());
            }
            
        }

        public void Animate(){
            StartCoroutine(Restart());
        }

        public IEnumerator Restart(){
            animator.SetBool("isSendingMagic", true);
            yield return new WaitForSecondsRealtime(0.5f);
            power.transform.position = transform.position;
            power.SendMessage("Shoot"); //Call a funtion/method of GameObject
            animator.SetBool("isSendingMagic", false);
        }

        public IEnumerator sendPowerCoroutine(){

            animator.SetBool("SendingPower", true);
            floorPower.Play();
            yield return new WaitForSecondsRealtime(1.0f);
            StartCoroutine(stopPowerCoroutine()); 
            animator.SetBool("SendingPower", false);             
        }


        public IEnumerator elevatePowerCoroutine(){

                animator.SetBool("ElevatePower", true);
                roundPower.Play();
                yield return new WaitForSecondsRealtime(1.0f);
                animator.SetBool("ElevatePower", false);
                StartCoroutine(stopPowerCoroutine());
                            
        }

        public IEnumerator stopPowerCoroutine(){
            yield return new WaitForSecondsRealtime(0.5f);
            floorPower.Stop();
            roundPower.Stop();
        }

    }
