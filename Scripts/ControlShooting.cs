using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlShooting : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject player;
    public float TimeForShoot = 1f;
    public float range = 100f;
    float timer; //Cronometro
    Ray shootRay;
    RaycastHit shootHit;
    int shootableMask;
    LineRenderer gunLine;
    Light gunLight;
    float effectsDisplayTime = 1.2f;


    /// Awake is called when the script instance is being loaded, before state grounded
    void Awake()
    {
        shootableMask = LayerMask.GetMask("Shootable"); //Obtain reference to layer Shootable
        gunLine = GetComponent<LineRenderer>(); //Obtain Component LineRenderer
        gunLight = GetComponent<Light>(); //
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= TimeForShoot * effectsDisplayTime)
        {
            DisableEffects(); //Stop to render the ray, duration of ray in render
        }
    }

    void Shoot()
    {   
        Vector3 ubication = new Vector3(player.transform.position.x, player.transform.position.y + 1.1f, player.transform.position.z); //Set position of shooter to the heigth of brazes of player
        timer = 0f; //Restart timer for effect of shoot, draw the shooting
        gunLine.enabled = true;
        gunLight.enabled = true;
        shootRay.origin = ubication; //Set position of ray 
        shootRay.direction = transform.forward; //Set direction of ray depending on front depending the front view of player
        gunLine.SetPosition(0, ubication); // Set position initial and final of Line

        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            //Destroy(shootHit.collider.gameObject);
            ControlResistance resistanceControl = shootHit.collider.gameObject.GetComponent<ControlResistance>();
            if(resistanceControl != null){
                resistanceControl.RegisterImpact(shootHit.point);
            }
            gunLine.SetPosition(1, shootHit.point);
        }
        else
        {
            Debug.Log("Without Impact X");
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }

    }

    void DisableEffects()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }
}
