using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    Collider Moonblade;

    // Start is called before the first frame update
    void Start()
    {
        Moonblade= GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Destructible")
        {
            
        }
    }
}
