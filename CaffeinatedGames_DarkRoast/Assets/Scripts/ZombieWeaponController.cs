using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieWeaponController : MonoBehaviour
{

    private float damage = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "hitbox") {
            Debug.Log("Player has been hit for " + damage + " damage.");
            PlayerHealth.instance.TakeDamage(damage);
        }
    }


    public void RightHandActive(float damage) {
        GetComponent<BoxCollider>().enabled = true;
        this.damage = damage;
        Debug.Log("RightHand has been activated");
    }

    public void RightHandInactive() {
        GetComponent<BoxCollider>().enabled = false;
        Debug.Log("RightHand has been de-activated");
    }

}
