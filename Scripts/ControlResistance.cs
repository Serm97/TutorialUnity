using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlResistance : MonoBehaviour
{

    public int resistance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RegisterImpact(Vector3 pointImpact){
        resistance--;

        if(resistance <= 0){
            Destroy(transform.gameObject);
        }
    }


    void OnParticleCollision(GameObject other)
    {
        Debug.Log("Colission Cube");
        Destroy(transform.gameObject);
    }
}
